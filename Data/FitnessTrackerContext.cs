using Microsoft.EntityFrameworkCore;
using FitnessTracker.Models;

namespace FitnessTracker.Data
{
    public class FitnessTrackerContext : DbContext
    {
        public FitnessTrackerContext(DbContextOptions<FitnessTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutCategory> WorkoutCategories { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<MealFood> MealFoods { get; set; }
        public DbSet<WaterIntake> WaterIntakes { get; set; }
        public DbSet<BodyMeasurement> BodyMeasurements { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // User relationships
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId);

            // Workout relationships
            modelBuilder.Entity<Workout>()
                .HasOne(w => w.User)
                .WithMany(u => u.Workouts)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Workout>()
                .HasOne(w => w.Category)
                .WithMany(c => c.Workouts)
                .HasForeignKey(w => w.CategoryId);

            // WorkoutExercise relationships
            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Workout)
                .WithMany(w => w.WorkoutExercises)
                .HasForeignKey(we => we.WorkoutId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkoutExercise>()
                .HasOne(we => we.Exercise)
                .WithMany()
                .HasForeignKey(we => we.ExerciseId);

            // Meal relationships
            modelBuilder.Entity<Meal>()
                .HasOne(m => m.User)
                .WithMany(u => u.Meals)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // MealFood relationships
            modelBuilder.Entity<MealFood>()
                .HasOne(mf => mf.Meal)
                .WithMany(m => m.MealFoods)
                .HasForeignKey(mf => mf.MealId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete conflicts

            modelBuilder.Entity<MealFood>()
                .HasOne(mf => mf.Food)
                .WithMany(f => f.MealFoods)
                .HasForeignKey(mf => mf.FoodId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete conflicts

            modelBuilder.Entity<MealFood>()
                .HasOne(mf => mf.User)
                .WithMany()
                .HasForeignKey(mf => mf.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete conflicts

            // Water intake relationship
            modelBuilder.Entity<WaterIntake>()
                .HasOne(w => w.User)
                .WithMany(u => u.WaterIntakes)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Body measurement relationship
            modelBuilder.Entity<BodyMeasurement>()
                .HasOne(b => b.User)
                .WithMany(u => u.BodyMeasurements)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Goal relationship
            modelBuilder.Entity<Goal>()
                .HasOne(g => g.User)
                .WithMany(u => u.Goals)
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Notification relationship
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Food user-created relationship
            modelBuilder.Entity<Food>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .IsRequired(false); // Allow nullable foreign key for system foods
        }
    }
}