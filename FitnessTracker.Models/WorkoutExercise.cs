using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace FitnessTracker.Models;
public class WorkoutExercise
{
    [Key]
    public int Id {get; set;}

    public int WorkoutId {get; set;}
    public required Workout Workout {get; set;}
    public int ExerciseId { get; set; }
    public required Exercise Exercise { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public decimal Weight { get; set; }

    public WorkoutExercise()
    {
    }
}
