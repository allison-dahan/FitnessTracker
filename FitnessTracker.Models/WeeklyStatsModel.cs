using System;
using System.Collections.Generic;
namespace FitnessTracker.Models;
public class WeeklyStatsModel
{
        public int Id {get; set;}
        public string Day { get; set; }
        public int Calories { get; set; }
        public int Steps { get; set; }
 }