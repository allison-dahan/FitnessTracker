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
    
    // Create tables first
    try {
        context.Database.ExecuteSqlRaw(@"
            CREATE TABLE IF NOT EXISTS `AspNetRoles` (
                `Id` varchar(255) NOT NULL,
                `Name` varchar(256) NULL,
                `NormalizedName` varchar(256) NULL,
                `ConcurrencyStamp` longtext NULL,
                CONSTRAINT `PK_AspNetRoles` PRIMARY KEY (`Id`)
            );
            
            CREATE TABLE IF NOT EXISTS `AspNetUsers` (
                `Id` varchar(255) NOT NULL,
                `FirstName` varchar(50) NULL,
                `LastName` varchar(50) NULL,
                `Gender` varchar(10) NULL,
                `City` varchar(100) NULL,
                `PostalCode` varchar(20) NULL,
                `DateOfBirth` datetime(6) NULL,
                `UserName` varchar(256) NULL,
                `NormalizedUserName` varchar(256) NULL,
                `Email` varchar(256) NULL,
                `NormalizedEmail` varchar(256) NULL,
                `EmailConfirmed` tinyint(1) NOT NULL,
                `PasswordHash` longtext NULL,
                `SecurityStamp` longtext NULL,
                `ConcurrencyStamp` longtext NULL,
                `PhoneNumber` longtext NULL,
                `PhoneNumberConfirmed` tinyint(1) NOT NULL,
                `TwoFactorEnabled` tinyint(1) NOT NULL,
                `LockoutEnd` datetime(6) NULL,
                `LockoutEnabled` tinyint(1) NOT NULL,
                `AccessFailedCount` int NOT NULL,
                CONSTRAINT `PK_AspNetUsers` PRIMARY KEY (`Id`)
            );
            
            CREATE TABLE IF NOT EXISTS `AspNetRoleClaims` (
                `Id` int NOT NULL AUTO_INCREMENT,
                `RoleId` varchar(255) NOT NULL,
                `ClaimType` longtext NULL,
                `ClaimValue` longtext NULL,
                CONSTRAINT `PK_AspNetRoleClaims` PRIMARY KEY (`Id`)
            );
            
            CREATE TABLE IF NOT EXISTS `AspNetUserClaims` (
                `Id` int NOT NULL AUTO_INCREMENT,
                `UserId` varchar(255) NOT NULL,
                `ClaimType` longtext NULL,
                `ClaimValue` longtext NULL,
                CONSTRAINT `PK_AspNetUserClaims` PRIMARY KEY (`Id`)
            );
            
            CREATE TABLE IF NOT EXISTS `AspNetUserLogins` (
                `LoginProvider` varchar(128) NOT NULL,
                `ProviderKey` varchar(128) NOT NULL,
                `ProviderDisplayName` longtext NULL,
                `UserId` varchar(255) NOT NULL,
                CONSTRAINT `PK_AspNetUserLogins` PRIMARY KEY (`LoginProvider`, `ProviderKey`)
            );
            
            CREATE TABLE IF NOT EXISTS `AspNetUserRoles` (
                `UserId` varchar(255) NOT NULL,
                `RoleId` varchar(255) NOT NULL,
                CONSTRAINT `PK_AspNetUserRoles` PRIMARY KEY (`UserId`, `RoleId`)
            );
            
            CREATE TABLE IF NOT EXISTS `AspNetUserTokens` (
                `UserId` varchar(255) NOT NULL,
                `LoginProvider` varchar(128) NOT NULL,
                `Name` varchar(128) NOT NULL,
                `Value` longtext NULL,
                CONSTRAINT `PK_AspNetUserTokens` PRIMARY KEY (`UserId`, `LoginProvider`, `Name`)
            );
        ");
    } catch (Exception ex) {
        logger.LogWarning(ex, "Error creating tables, they might already exist");
    }
    
    // Add foreign keys and indexes in separate try/catch blocks
    try { context.Database.ExecuteSqlRaw("ALTER TABLE `AspNetRoleClaims` ADD CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE;"); } 
    catch (Exception ex) { logger.LogInformation("Foreign key already exists: " + ex.Message); }
    
    try { context.Database.ExecuteSqlRaw("ALTER TABLE `AspNetUserClaims` ADD CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE;"); } 
    catch (Exception ex) { logger.LogInformation("Foreign key already exists: " + ex.Message); }
    
    try { context.Database.ExecuteSqlRaw("ALTER TABLE `AspNetUserLogins` ADD CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE;"); } 
    catch (Exception ex) { logger.LogInformation("Foreign key already exists: " + ex.Message); }
    
    try { context.Database.ExecuteSqlRaw("ALTER TABLE `AspNetUserRoles` ADD CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE;"); } 
    catch (Exception ex) { logger.LogInformation("Foreign key already exists: " + ex.Message); }
    
    try { context.Database.ExecuteSqlRaw("ALTER TABLE `AspNetUserRoles` ADD CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE;"); } 
    catch (Exception ex) { logger.LogInformation("Foreign key already exists: " + ex.Message); }
    
    try { context.Database.ExecuteSqlRaw("ALTER TABLE `AspNetUserTokens` ADD CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE;"); } 
    catch (Exception ex) { logger.LogInformation("Foreign key already exists: " + ex.Message); }
    
    // Create indexes in separate try/catch blocks
    try { context.Database.ExecuteSqlRaw("CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON `AspNetRoleClaims` (`RoleId`);"); } 
    catch (Exception ex) { logger.LogInformation("Index already exists: " + ex.Message); }
    
    try { context.Database.ExecuteSqlRaw("CREATE UNIQUE INDEX `RoleNameIndex` ON `AspNetRoles` (`NormalizedName`);"); } 
    catch (Exception ex) { logger.LogInformation("Index already exists: " + ex.Message); }
    
    try { context.Database.ExecuteSqlRaw("CREATE INDEX `IX_AspNetUserClaims_UserId` ON `AspNetUserClaims` (`UserId`);"); } 
    catch (Exception ex) { logger.LogInformation("Index already exists: " + ex.Message); }
    
    try { context.Database.ExecuteSqlRaw("CREATE INDEX `IX_AspNetUserLogins_UserId` ON `AspNetUserLogins` (`UserId`);"); } 
    catch (Exception ex) { logger.LogInformation("Index already exists: " + ex.Message); }
    
    try { context.Database.ExecuteSqlRaw("CREATE INDEX `IX_AspNetUserRoles_RoleId` ON `AspNetUserRoles` (`RoleId`);"); } 
    catch (Exception ex) { logger.LogInformation("Index already exists: " + ex.Message); }
    
    try { context.Database.ExecuteSqlRaw("CREATE INDEX `EmailIndex` ON `AspNetUsers` (`NormalizedEmail`);"); } 
    catch (Exception ex) { logger.LogInformation("Index already exists: " + ex.Message); }
    
    try { context.Database.ExecuteSqlRaw("CREATE UNIQUE INDEX `UserNameIndex` ON `AspNetUsers` (`NormalizedUserName`);"); } 
    catch (Exception ex) { logger.LogInformation("Index already exists: " + ex.Message); }
    
    logger.LogInformation("Database schema verified");
    
    // Now try to create roles
    try
    {
        CreateRoles(services).Wait();
        logger.LogInformation("Roles created successfully");
    }
    catch (Exception roleEx)
    {
        logger.LogError(roleEx, "Error creating roles, but continuing application startup");
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