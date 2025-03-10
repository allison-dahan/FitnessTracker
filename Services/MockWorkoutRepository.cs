using FitnessTracker.Models;

namespace FitnessTracker.Services
{
    public class MockWorkoutRepository : IWorkoutRepository
    {
        private List<Workout> _workouts;
        private List<Exercise> _exercises;
        private List<WorkoutCategory> _categories;

        public MockWorkoutRepository()
        {
            _categories = new List<WorkoutCategory>
            {
                new WorkoutCategory { Id = 1, Name = "Strength Training", Description = "Resistance exercises for building muscle" },
                new WorkoutCategory { Id = 2, Name = "Cardio", Description = "Cardiovascular exercises for endurance" },
                new WorkoutCategory { Id = 3, Name = "Flexibility", Description = "Stretching exercises for mobility" }
            };
            
            _exercises = new List<Exercise>
            {
                new Exercise { 
                    Id = 1, 
                    Name = "Bench Press",
                    Category = "Strength",
                    Description = "Barbell bench press for chest",
                    TargetMuscleGroup = "Chest"
                },
                new Exercise { 
                    Id = 2, 
                    Name = "Squats",
                    Category = "Strength",
                    Description = "Barbell squats for legs",
                    TargetMuscleGroup = "Legs"
                },
                new Exercise {
                    Id = 3,
                    Name = "Running",
                    Category = "Cardio",
                    Description = "Keeping",
                    TargetMuscleGroup = "Cardio"
                }
            };

            _workouts = new List<Workout>
            {
                new Workout {
                    Id = 1,
                    UserId = 1,
                    CategoryId = 1,
                    Category = _categories[0],
                    Date = DateTime.Now,
                    Type = "Strength Training",
                    Duration = 60,
                    WorkoutExercises = new List<WorkoutExercise>
                    {
                        new WorkoutExercise {
                            Id = 1,
                            WorkoutId = 1,
                            ExerciseId = 1,
                            Exercise = _exercises[0],
                            Sets = 3,
                            Reps = 10,
                            Weight = 135
                        }
                    }
                }
            };
        }

        public IEnumerable<Workout> GetAllWorkouts() => _workouts;
        public IEnumerable<Exercise> GetAllExercises() => _exercises;
        public Workout GetWorkoutById(int id) => _workouts.FirstOrDefault(w => w.Id == id);

        public void AddWorkout(Workout workout)
        {
            workout.Id = _workouts.Any() ? _workouts.Max(w => w.Id) + 1 : 1;
            workout.UserId = 1; // Default user ID
            
            // Link workout to category
            if (workout.CategoryId > 0)
            {
                workout.Category = _categories.FirstOrDefault(c => c.Id == workout.CategoryId);
            }
            
            // Process workout exercises
            if (workout.WorkoutExercises != null)
            {
                foreach (var we in workout.WorkoutExercises)
                {
                    we.Id = 0; // Reset ID so it will be auto-assigned
                    we.WorkoutId = workout.Id;
                    we.Exercise = _exercises.FirstOrDefault(e => e.Id == we.ExerciseId);
                }
            }
            
            _workouts.Add(workout);
        }

        public void UpdateWorkout(Workout workout)
        {
            var existing = _workouts.FirstOrDefault(w => w.Id == workout.Id);
            if (existing != null)
            {
                existing.Date = workout.Date;
                existing.Type = workout.Type;
                existing.Duration = workout.Duration;
                existing.Notes = workout.Notes;
                existing.CategoryId = workout.CategoryId;
                existing.Category = _categories.FirstOrDefault(c => c.Id == workout.CategoryId);
                
                // Update exercises
                if (workout.WorkoutExercises != null)
                {
                    existing.WorkoutExercises = workout.WorkoutExercises;
                    foreach (var we in existing.WorkoutExercises)
                    {
                        we.WorkoutId = existing.Id;
                        we.Exercise = _exercises.FirstOrDefault(e => e.Id == we.ExerciseId);
                    }
                }
            }
        }

        public void DeleteWorkout(int id)
        {
            var workout = _workouts.FirstOrDefault(w => w.Id == id);
            if (workout != null) _workouts.Remove(workout);
        }

        public IEnumerable<Workout> GetRecentWorkouts(int count = 5)
        {
            return _workouts.OrderByDescending(w => w.Date).Take(count);
        }
    }
}