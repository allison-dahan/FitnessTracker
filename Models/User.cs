using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PasswordHash { get; set; }
        
        public DateTime DateRegistered { get; set; } = DateTime.Now;
        
        public DateTime? LastLogin { get; set; }
        
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public UserProfile Profile { get; set; }
        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
        public ICollection<Meal> Meals { get; set; } = new List<Meal>();
        public ICollection<BodyMeasurement> BodyMeasurements { get; set; } = new List<BodyMeasurement>();
        public ICollection<WaterIntake> WaterIntakes { get; set; } = new List<WaterIntake>();
        public ICollection<Goal> Goals { get; set; } = new List<Goal>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}