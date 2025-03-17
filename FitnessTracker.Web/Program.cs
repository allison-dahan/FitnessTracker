using FitnessTracker.DataAccess.Interfaces;
using FitnessTracker.DataAccess.Repositories;
using FitnessTracker.Business.Services;
using FitnessTracker.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Registering the Profile Service
builder.Services.AddScoped<IProfileService, ProfileService>();

// Registering Repositories (Choose Mock or Database based on environment)
if (builder.Environment.IsDevelopment())
{
    // Use mock repository in development
    builder.Services.AddScoped<IProfileRepository, MockProfileRepository>();
}
else
{
    // Use database repository in production
    builder.Services.AddScoped<IProfileRepository, ProfileDatabaseRepository>();
}

// Database Context
builder.Services.AddDbContext<FitnessTrackerContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 2, 0))
    ));

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
