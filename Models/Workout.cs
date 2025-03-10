using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Models
{
    public class Workout
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
        
        public int CategoryId { get; set; }
        
        [ForeignKey("CategoryId")]
        public WorkoutCategory Category { get; set; }
        
        public string Name { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        public string Type { get; set; }
        
        [Range(1, 300)]
        public int Duration { get; set; }  // In minutes
        
        public int? CaloriesBurned { get; set; }
        
        public string Notes { get; set; }
        
        public bool IsCompleted { get; set; } = true;

        // Navigation property
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; } = new List<WorkoutExercise>();
    }
}