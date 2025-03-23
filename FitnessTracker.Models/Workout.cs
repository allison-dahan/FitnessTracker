using System;
using System.Collections.Generic;
namespace FitnessTracker.Models;
public class Workout
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Type { get; set; }  // Training type
    public List<WorkoutExercise> WorkoutExercises { get; set; }
    public int Duration { get; set; }  // In minutes
    public string Notes { get; set; }

    public Workout()
    {
        WorkoutExercises = new List<WorkoutExercise>();  // Initialize the Exercises list
    }
}