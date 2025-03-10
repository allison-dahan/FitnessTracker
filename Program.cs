using FitnessTracker.Services;
using FitnessTracker.Repositories.Implementations;
using FitnessTracker.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure database context for storage memory (database)
builder.Services.AddDbContext<FitnessTrackerContext>(options => 
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// // In-Memory Repositories (Mock implementations)
// builder.Services.AddScoped<IWorkoutRepository, MockWorkoutRepository>();
// builder.Services.AddScoped<IStatsRepository, MockStatsRepository>();
// builder.Services.AddScoped<INutritionRepository, MockNutritionRepository>();
// builder.Services.AddScoped<IProfileRepository, MockProfileRepository>();

// // Repository Wrappers
// builder.Services.AddScoped<WorkoutRepositoryWrapper>();
// builder.Services.AddScoped<StatsRepositoryWrapper>();
// builder.Services.AddScoped<NutritionRepositoryWrapper>();
// builder.Services.AddScoped<ProfileRepositoryWrapper>();
// Keep mock repositories for reference or potential future use
builder.Services.AddScoped<MockWorkoutRepository>();
builder.Services.AddScoped<MockStatsRepository>();
builder.Services.AddScoped<MockNutritionRepository>();
builder.Services.AddScoped<MockProfileRepository>();

// Primary repositories using database context
builder.Services.AddScoped<IWorkoutRepository, WorkoutRepository>();
builder.Services.AddScoped<IStatsRepository, StatsRepository>();
builder.Services.AddScoped<INutritionRepository, NutritionRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();

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

// Ensure database is created with relationships
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FitnessTrackerContext>();
    context.Database.EnsureCreated(); // Creates database if not exists
}

app.Run();