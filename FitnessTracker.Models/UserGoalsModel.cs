using System;
using System.Collections.Generic;
namespace FitnessTracker.Models;

public class UserGoalsModel
{
    public decimal WeightGoal {get; set;}
    public int CaloriesPerDay {get; set;}
    public int WorkoutsPerWeek {get; set;}
    public int StepsPerDay {get; set;}
}