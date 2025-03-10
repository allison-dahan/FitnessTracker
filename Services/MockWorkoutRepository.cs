using FitnessTracker.Models;

namespace FitnessTracker.Services;

public class MockWorkoutRepository : IWorkoutRepository

{
    private List<Workout> _workouts;
    private List<Exercise> _exercises;

    public MockWorkoutRepository()
    {
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
                Date = DateTime.Now,
                Type = "Strength Training",
                Duration = 60,
                Exercises = new List<WorkoutExercise>
                {
                    new WorkoutExercise {
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
        workout.Id = _workouts.Max(w => w.Id) + 1;
        _workouts.Add(workout);
    }

    public void UpdateWorkout(Workout workout)
    {
        var existing = _workouts.FirstOrDefault(w => w.Id == workout.Id);
        if (existing != null)
        {
            existing.Date = workout.Date;
            existing.Type = workout.Type;
            existing.Exercises = workout.Exercises;
            existing.Duration = workout.Duration;
            existing.Notes = workout.Notes;
        }
    }

    public void DeleteWorkout(int id)
    {
        var workout = _workouts.FirstOrDefault(w => w.Id == id);
        if (workout != null) _workouts.Remove(workout);
    }

    public IEnumerable<Workout> GetRecentWorkouts(int count)
    {
        return _workouts.OrderByDescending(w => w.Date).Take(count);
    }
}