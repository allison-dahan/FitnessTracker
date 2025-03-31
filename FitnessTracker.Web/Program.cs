using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;

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
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// Set minimum log levels based on environment
if (builder.Environment.IsDevelopment())
{
    builder.Logging.SetMinimumLevel(LogLevel.Debug);
}
else if (builder.Environment.IsStaging())
{
    builder.Logging.SetMinimumLevel(LogLevel.Warning);
}
else // Production
{
    builder.Logging.SetMinimumLevel(LogLevel.Error);
}
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
    string connectionString = builder.Configuration.GetConnectionString("Development");

    if (builder.Environment.IsStaging())
    {
        connectionString = builder.Configuration.GetConnectionString("Staging");

    } else if (builder.Environment.IsProduction())
    {
        connectionString = builder.Configuration.GetConnectionString("Production");
    }
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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Add Authentication and Authorization middleware
app.UseAuthentication();
app.UseAuthorization(); 

// Logging configuration
app.UseHttpLogging();

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
        
        // Ensure database is created
        context.Database.EnsureCreated();
        logger.LogInformation($"Database ensured created in {app.Environment.EnvironmentName} environment.");

        // // Seed data if database is empty
        // SeedData(context, logger);
        
        // Optionally create default roles
        //CreateRoles(services).Wait();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, $"An error occurred while initializing the database in {app.Environment.EnvironmentName} environment.");
        throw;
    }
}

app.Run();

// Uncomment and implement this method if you want to create default roles
/*
private static async Task CreateRoles(IServiceProvider serviceProvider)
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
    
    // Here you can also create an admin user if needed
}
*/