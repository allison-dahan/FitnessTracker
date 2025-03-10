using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Models
{
    public class WaterIntake
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        [Required]
        [Range(0.1, 2.0, ErrorMessage = "Amount must be between 0.1 and 2.0 liters")]
        [Display(Name = "Amount (L)")]
        public decimal Amount { get; set; }
        
        public DateTime TimeEntered { get; set; } = DateTime.Now;
    }
}