using System.ComponentModel.DataAnnotations;
using FitnessTracker.Models.Identity;
namespace FitnessTracker.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        // Add this property to link to Identity user
        public string IdentityUserId { get; set; }

        // This is optional but recommended for easier navigation in code
        public virtual ApplicationUser IdentityUser { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = "default@example.com";

        [Range(13, 100, ErrorMessage = "Age must be between 13 and 100")]
        public int Age { get; set; }

        public string Gender { get; set; }

        [Display(Name = "Height (cm)")]
        [Range(100, 250, ErrorMessage = "Height must be between 100 and 250 cm")]
        public decimal Height { get; set; }

        [Display(Name = "Weight (kg)")]
        [Range(30, 300, ErrorMessage = "Weight must be between 30 and 300 kg")]
        public decimal Weight { get; set; }

        [Display(Name = "Fitness Goal")]
        public string FitnessGoal { get; set; }

        [Display(Name = "Target Weight (kg)")]
        [Range(30, 300, ErrorMessage = "Target weight must be between 30 and 300 kg")]
        public decimal TargetWeight { get; set; }
    }
}