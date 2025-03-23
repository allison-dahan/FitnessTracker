using System;
using System.Collections.Generic;
namespace FitnessTracker.Models;
public class WorkoutViewModel
{
    public required Workout Workout { get; set; }
    public required List<Exercise> AvailableExercises { get; set; }
    public required List<string> WorkoutTypes { get; set; }
}