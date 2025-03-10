using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Models
{
    public class Notification
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
        
        public string Type { get; set; } // Goal, Reminder, System
        
        [Required]
        public string Message { get; set; }
        
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        
        public bool IsRead { get; set; } = false;
        
        public string LinkUrl { get; set; }
    }
}