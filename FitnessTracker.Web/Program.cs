using FitnessTracker.DataAccess.Interfaces;
using FitnessTracker.DataAccess.Repositories;
using FitnessTracker.Business.Services;
using FitnessTracker.DataAccess;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();



// Database Context
builder.Services.AddDbContext<FitnessTrackerContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))
    ));

// Repositories (Database)
builder.Services.AddScoped<IProfileRepository, ProfileDatabaseRepository>();
builder.Services.AddScoped<INutritionRepository, NutritionDatabaseRepository>();
builder.Services.AddScoped<IWorkoutRepository, WorkoutDatabaseRepository>();
builder.Services.AddScoped<IStatsRepository, StatsDatabaseRepository>();

// Services 
builder.Services.AddScoped<IProfileService, ProfileService>();

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
// app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Optional: Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FitnessTrackerContext>();
    context.Database.EnsureCreated();
}

app.Run();