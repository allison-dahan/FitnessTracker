using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Models.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // Add additional user properties
        [PersonalData]
        [MaxLength(50)]
        public string? FirstName { get; set; }
        
        [PersonalData]
        [MaxLength(50)]
        public string? LastName { get; set; }
        
        [PersonalData]
        [MaxLength(10)]
        public string? Gender { get; set; }
        
        [PersonalData]
        [MaxLength(100)]
        public string? City { get; set; }
        
        [PersonalData]
        [MaxLength(20)]
        public string? PostalCode { get; set; }
        
        [PersonalData]
        public DateTime? DateOfBirth { get; set; }
    }
}
