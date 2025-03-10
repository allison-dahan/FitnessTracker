using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Models
{
    public class MealFood
    {
        public int Id { get; set; }
        
        public int MealId { get; set; }
        
        [ForeignKey("MealId")]
        public Meal Meal { get; set; }
        
        public int FoodId { get; set; }
        
        [ForeignKey("FoodId")]
        public Food Food { get; set; }
        
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
        
        [Range(0.25, 10)]
        public decimal Servings { get; set; }
        
        public string Notes { get; set; }
        
        public DateTime DateAdded { get; set; } = DateTime.Now;
    }
}