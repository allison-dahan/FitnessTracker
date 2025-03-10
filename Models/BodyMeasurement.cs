using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Models
{
    public class BodyMeasurement
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        
        [ForeignKey("UserId")]
        public User User { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        [Range(30, 300)]
        public decimal Weight { get; set; }
        
        [Range(1, 80)]
        public decimal? BodyFatPercentage { get; set; }
        
        public decimal? ChestCircumference { get; set; }
        
        public decimal? WaistCircumference { get; set; }
        
        public decimal? HipCircumference { get; set; }
        
        public decimal? BicepsCircumference { get; set; }
        
        public decimal? ThighCircumference { get; set; }
        
        public string Notes { get; set; }
    }
}