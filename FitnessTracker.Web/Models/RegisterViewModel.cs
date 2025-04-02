using System.ComponentModel.DataAnnotations;
namespace FitnessTracker.Web.Models;
public class RegisterViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 8)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "Passwords don't match")]
    public string ConfirmPassword { get; set; }

    // Add fields for your custom user properties
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    
    [Display(Name = "Last Name")]
    public string LastName { get; set; }
    
    public string Gender { get; set; }
    
    public string City { get; set; }
    
    [Display(Name = "Postal Code")]
    public string PostalCode { get; set; }
    
    [Display(Name = "Date of Birth")]
    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }
}