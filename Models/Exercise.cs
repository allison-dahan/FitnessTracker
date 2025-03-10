using System;
using System.Collections.Generic;
namespace FitnessTracker.Models;

public class Exercise
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Category { get; set; }  // Strength, Cardio, Flexibility
    public required string Description { get; set; }
    public required string TargetMuscleGroup { get; set; }
}