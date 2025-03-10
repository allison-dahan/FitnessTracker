using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Models
{
    public class Food
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string ServingSize { get; set; }
        
        public int CaloriesPerServing { get; set; }
        
        public int ProteinPerServing { get; set; }
        
        public int CarbsPerServing { get; set; }
        
        public int FatPerServing { get; set; }
        
        public bool IsUserCreated { get; set; } = false;
        
        public int? UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
        
        // Navigation property
        public ICollection<MealFood> MealFoods { get; set; } = new List<MealFood>();
    }
}