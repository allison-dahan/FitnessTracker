using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;


namespace FitnessTracker.Models
{
    public class DashboardViewModel
    {
        public DailyStatsModel DailyStats {get;  set;}
        public IEnumerable<Workout> RecentWorkouts {get; set;}
        public NutritionSummaryModel NutritionSummary {get; set;}
    }

    public class DailyStatsModel
    {
        public int CaloriesConsumed {get; set;}
        public int CaloriesGoal {get; set; }
        public int WorkoutsThisWeek {get; set; }
        public int WorkoutsGoal {get; set;}
        public decimal WaterIntake {get; set; } // Liters
        public decimal WaterGoal {get; set;}
        public int StepsToday {get; set; }
        public int StepsGoal {get; set; }
    }

public class NutritionSummaryModel
    {
        [Display(Name = "Total Calories")]
        public int TotalCalories { get; set; }
        
        [Display(Name = "Calorie Goal")]
        public int CalorieGoal { get; set; }
        
        [Display(Name = "Protein (g)")]
        public int ProteinGrams { get; set; }
        
        [Display(Name = "Protein Goal (g)")]
        public int ProteinGoal { get; set; }
        
        [Display(Name = "Carbs (g)")]
        public int CarbsGrams { get; set; }
        
        [Display(Name = "Carbs Goal (g)")]
        public int CarbsGoal { get; set; }
        
        [Display(Name = "Fat (g)")]
        public int FatGrams { get; set; }
        
        [Display(Name = "Fat Goal (g)")]
        public int FatGoal { get; set; }
        
        [Display(Name = "Water Intake (L)")]
        public decimal WaterIntake { get; set; }
        
        [Display(Name = "Water Goal (L)")]
        public decimal WaterGoal { get; set; }
    }

    public class WaterIntakeModel
    {
        public int Id { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        [Range(0.1, 2.0, ErrorMessage = "Amount must be between 0.1 and 2.0 liters")]
        [Display(Name = "Amount (L)")]
        public decimal Amount { get; set; }
    }
}