namespace FitnessTracker.Services;
using FitnessTracker.Models;

public interface IWorkoutRepository
{
    IEnumerable<Workout> GetAllWorkouts();
    Workout GetWorkoutById(int id);
    void AddWorkout(Workout workout);
    void UpdateWorkout(Workout workout);
    void DeleteWorkout(int id);
    IEnumerable<Exercise> GetAllExercises();

    IEnumerable<Workout> GetRecentWorkouts(int count);
}
