using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Google;

using FitnessTracker.DataAccess;
using FitnessTracker.DataAccess.Interfaces;
using FitnessTracker.DataAccess.Repositories;
using FitnessTracker.Business.Services;
using FitnessTracker.Models;
using FitnessTracker.Models.Identity; // Make sure this namespace is correct
using FitnessTracker.Web.Middleware;
using FitnessTracker.Web.Identity; // For CustomPasswordValidator

var builder = WebApplication.CreateBuilder(args);

//Configuration Management
builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

// Set minimum log levels based on environment
// Set log level
builder.Logging.SetMinimumLevel(LogLevel.Debug);
// Add services to the container
builder.Services.AddControllersWithViews();

// Logging configuration
builder.Logging.ClearProviders();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Database Context
builder.Services.AddDbContext<FitnessTrackerContext>(options =>
{
    // Use a single connection string for all environments
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
                              ?? builder.Configuration.GetConnectionString("Development");
    options.UseMySql(
        connectionString,
        new MySqlServerVersion(new Version(8, 0, 21)),
        mysqlOptions =>
        {
            mysqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }
    );
});

// *** ASP.NET Core Identity Configuration ***
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequiredLength = 8;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    
    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<FitnessTrackerContext>()  // Use your existing context
.AddDefaultTokenProviders()
.AddPasswordValidator<CustomPasswordValidator<ApplicationUser>>();

// Add this in Program.cs where you configure services
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        options.CallbackPath = "/signin-google";
    });

// Repositories (Database)
builder.Services.AddScoped<IProfileRepository, ProfileDatabaseRepository>();
builder.Services.AddScoped<INutritionRepository, NutritionDatabaseRepository>();
builder.Services.AddScoped<IWorkoutRepository, WorkoutDatabaseRepository>();
builder.Services.AddScoped<IStatsRepository, StatsDatabaseRepository>();

// Services 
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddHealthChecks();

builder.Services.AddHttpLogging(options => 
{
    options.LoggingFields = HttpLoggingFields.All;
    options.RequestHeaders.Add("sec-ch-ua");
    options.ResponseHeaders.Add("my-response-header");
    options.MediaTypeOptions.AddText("application/javascript");
    options.RequestBodyLogLimit = 4096;
    options.ResponseBodyLogLimit = 4096;
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseHttpLogging();
}
else
{
    app.UseGlobalExceptionHandler();
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Add Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization(); 

// Logging configuration
app.UseHttpLogging();

app.MapControllerRoute(
    name: "spa",
    pattern: "spa/{*catchall}",
    defaults: new { controller = "Spa", action = "Index" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Add health check endpoint
app.MapHealthChecks("/health");

// Ensure database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try 
{
    var context = services.GetRequiredService<FitnessTrackerContext>();
    
    // Create database and tables using EF Core
    // This handles creation if not exists, and ignores if exists.
    // It's much safer than the manual SQL script.
    context.Database.EnsureCreated();

    logger.LogInformation("Database schema verified via EnsureCreated");
    
    // Now try to create roles
    try
    {
        await CreateRoles(services);
        logger.LogInformation("Roles created successfully");
    }
    catch (Exception roleEx)
    {
        logger.LogError(roleEx, "Error creating roles, but continuing application startup");
    }
    // Seed real data for charts
    try
    {
        await SeedData(services);
        logger.LogInformation("Data seeded successfully");
    }
    catch (Exception seedEx)
    {
        logger.LogError(seedEx, "Error seeding data");
    }
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occurred while initializing the database");
}

}

app.Run();


async Task CreateRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    
    string[] roleNames = { "Admin", "User", "Trainer" };
    
    foreach (var roleName in roleNames)
    {
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }
    
    // Create admin user
    var adminEmail = "admin@fitnessapp.com";
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    
    if (adminUser == null)
    {
        adminUser = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            FirstName = "Admin",
            LastName = "User",
            EmailConfirmed = true
        };
        
        var result = await userManager.CreateAsync(adminUser, "Admin@123456");
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}

async Task SeedData(IServiceProvider serviceProvider)
{
    var context = serviceProvider.GetRequiredService<FitnessTrackerContext>();
    
    // 1. Seed Workouts if none exist
    if (!context.Workouts.Any())
    {
        var today = DateTime.Now;
        var workouts = new List<Workout>
        {
            new Workout { Date = today.AddDays(-6), Type = "Running", Duration = 30, Notes = "Morning jog, approx 300 kcal" },
            new Workout { Date = today.AddDays(-5), Type = "Weightlifting", Duration = 45, Notes = "Upper body, approx 250 kcal" },
            new Workout { Date = today.AddDays(-4), Type = "Yoga", Duration = 60, Notes = "Relaxing flow, approx 200 kcal" },
            new Workout { Date = today.AddDays(-2), Type = "HIIT", Duration = 25, Notes = "Quick session, approx 350 kcal" },
            new Workout { Date = today.AddDays(-1), Type = "Swimming", Duration = 40, Notes = "Laps, approx 400 kcal" },
            new Workout { Date = today, Type = "Cycling", Duration = 50, Notes = "Evening ride, approx 450 kcal" }
        };
        
        context.Workouts.AddRange(workouts);
        await context.SaveChangesAsync();
    }

    // 2. Seed Meals if none exist (for the last week)
    if (!context.Meals.Any())
    {
        var today = DateTime.Now;
        var meals = new List<MealModel>();

        for(int i = 6; i >= 0; i--)
        {
            var date = today.AddDays(-i);
            // Breakfast
            meals.Add(new MealModel { Date = date, MealType = "Breakfast", Calories = 350, Protein = 12, Carbs = 60, Fat = 6, Notes = "Oatmeal" });
            // Lunch
            meals.Add(new MealModel { Date = date, MealType = "Lunch", Calories = 450, Protein = 40, Carbs = 10, Fat = 20, Notes = "Chicken Salad" });
            // Dinner
            meals.Add(new MealModel { Date = date, MealType = "Dinner", Calories = 600, Protein = 35, Carbs = 50, Fat = 25, Notes = "Salmon & Rice" });
        }
        
        context.Meals.AddRange(meals);
        await context.SaveChangesAsync();
    }
}