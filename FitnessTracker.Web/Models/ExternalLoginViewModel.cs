using System.ComponentModel.DataAnnotations;
namespace FitnessTracker.Web.Models;
public class ExternalLoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    // This property will hold the name of the provider (e.g., "Google")
    public string ProviderDisplayName { get; set; }
}