using FitnessTracker.DataAccess.Interfaces;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.DataAccess.Repositories
{
    public class WorkoutDatabaseRepository : IWorkoutRepository
    {
        private readonly FitnessTrackerContext _context;

        public WorkoutDatabaseRepository(FitnessTrackerContext context)
        {
            _context = context;
        }

        public IEnumerable<Workout> GetAllWorkouts()
        {
            return _context.Workouts
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                .ToList();
        }

        public Workout GetWorkoutById(int id)
        {
            return _context.Workouts
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                .FirstOrDefault(w => w.Id == id);
        }

        public void AddWorkout(Workout workout)
        {
            _context.Workouts.Add(workout);
            _context.SaveChanges();
        }

        public void UpdateWorkout(Workout workout)
        {
            var existingWorkout = _context.Workouts
                .Include(w => w.WorkoutExercises)
                .FirstOrDefault(w => w.Id == workout.Id);

            if (existingWorkout != null)
            {
                existingWorkout.Date = workout.Date;
                existingWorkout.Type = workout.Type;
                existingWorkout.Duration = workout.Duration;
                existingWorkout.Notes = workout.Notes;

                // Update or add workout exercises
                existingWorkout.WorkoutExercises = workout.WorkoutExercises;

                _context.SaveChanges();
            }
        }

        public void DeleteWorkout(int id)
        {
            var workout = _context.Workouts.Find(id);
            if (workout != null)
            {
                _context.Workouts.Remove(workout);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Exercise> GetAllExercises()
        {
            return _context.Exercises.ToList();
        }

        public IEnumerable<Workout> GetRecentWorkouts(int count)
        {
            return _context.Workouts
                .OrderByDescending(w => w.Date)
                .Take(count)
                .ToList();
        }
    }
}