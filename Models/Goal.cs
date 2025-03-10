using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Models
{
    public class Goal
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
        
        [Required]
        public string GoalType { get; set; } // Weight, Workout, Nutrition, etc.
        
        public decimal TargetValue { get; set; }
        
        public decimal StartValue { get; set; }
        
        public DateTime StartDate { get; set; } = DateTime.Now;
        
        public DateTime TargetDate { get; set; }
        
        public DateTime? CompletionDate { get; set; }
        
        public string Status { get; set; } = "In Progress"; // In Progress, Completed, Abandoned
        
        public string Notes { get; set; }
    }
}