using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Models;
using FitnessTracker.Models.Identity; // Assuming this is where your ApplicationUser is

namespace FitnessTracker.DataAccess
{
    public class FitnessTrackerContext : IdentityDbContext<ApplicationUser>
    {
        // Constructor that takes DbContextOptions
        public FitnessTrackerContext(DbContextOptions<FitnessTrackerContext> options)
            : base(options)
        {
        }

        // DbSet properties for each of your models
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<MealModel> Meals { get; set; }
        public DbSet<WaterIntakeModel> WaterIntakes { get; set; }
        public DbSet<WeeklyStatsModel> WeeklyStats { get; set; }
        public DbSet<DailyStatsModel> DailyStats { get; set; }

        // Optional: Configure relationships and constraints
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Call base implementation first to set up Identity tables
            base.OnModelCreating(modelBuilder);

            // Workout to WorkoutExercises relationship
            modelBuilder.Entity<Workout>()
                .HasMany(w => w.WorkoutExercises)
                .WithOne(we => we.Workout)
                .HasForeignKey(we => we.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);

            // Exercise to WorkoutExercises relationship
            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Exercise)
                .WithMany()
                .HasForeignKey(we => we.ExerciseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserProfile>()
                .HasKey(p => p.Id);

            // add a relationship between ApplicationUser and UserProfile

            modelBuilder.Entity<ApplicationUser>()
                .HasOne<UserProfile>()
                .WithOne()
                .HasForeignKey<UserProfile>(up => up.IdentityUserId);
            
            











            
            // You can also customize the Identity tables here if needed
            // For example, to change table names:
            /*
            modelBuilder.Entity<ApplicationUser>().ToTable("Users");
            modelBuilder.Entity<IdentityRole>().ToTable("Roles");
            */
        }
    }
}