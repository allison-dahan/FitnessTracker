using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Models
{
    public class WorkoutExercise
    {
        public int Id { get; set; }
        
        public int WorkoutId { get; set; }
        
        [ForeignKey("WorkoutId")]
        public Workout Workout { get; set; }
        
        public int ExerciseId { get; set; }
        
        [ForeignKey("ExerciseId")]
        public Exercise Exercise { get; set; }
        
        [Range(0, 100)]
        public int Sets { get; set; }
        
        [Range(0, 1000)]
        public int Reps { get; set; }
        
        [Range(0, 1000)]
        public decimal Weight { get; set; }
        
        public int? Duration { get; set; }
        
        public int? RestInterval { get; set; }
        
        public string Notes { get; set; }
    }
}