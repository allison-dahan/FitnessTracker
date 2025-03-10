using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Models
{
    public class Meal
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
        
        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }
        
        [Required]
        [Display(Name = "Meal Type")]
        public string MealType { get; set; } // Breakfast, Lunch, Dinner, Snack
        
        [Required]
        [Range(0, 5000, ErrorMessage = "Calories must be between 0 and 5000")]
        public int TotalCalories { get; set; }
        
        [Range(0, 500, ErrorMessage = "Protein must be between 0 and 500g")]
        [Display(Name = "Protein (g)")]
        public int Protein { get; set; }
        
        [Range(0, 1000, ErrorMessage = "Carbs must be between 0 and 1000g")]
        [Display(Name = "Carbs (g)")]
        public int Carbs { get; set; }
        
        [Range(0, 300, ErrorMessage = "Fat must be between 0 and 300g")]
        [Display(Name = "Fat (g)")]
        public int Fat { get; set; }
        
        [MaxLength(500)]
        public string Notes { get; set; }
        
        // Navigation property
        public ICollection<MealFood> MealFoods { get; set; } = new List<MealFood>();
    }
}