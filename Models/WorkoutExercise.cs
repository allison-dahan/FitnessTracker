using System;
using System.Collections.Generic;
namespace FitnessTracker.Models;
public class WorkoutExercise
{
    public int ExerciseId { get; set; }
    public required Exercise Exercise { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public decimal Weight { get; set; }
}