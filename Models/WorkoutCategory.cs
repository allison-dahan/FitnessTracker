using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Models
{
    public class WorkoutCategory
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public string ImageUrl { get; set; }
        
        // Navigation property
        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
    }
}