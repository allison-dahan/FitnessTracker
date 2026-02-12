using System.ComponentModel.DataAnnotations;
namespace FitnessTracker.Web.Models;
// RoleViewModel.cs
public class RoleViewModel
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}

// CreateRoleViewModel.cs
public class CreateRoleViewModel
{
    [Required]
    [Display(Name = "Role Name")]
    public string RoleName { get; set; } = string.Empty;
}

// EditRoleViewModel.cs
public class EditRoleViewModel
{
    public string Id { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Role Name is required")]
    public string RoleName { get; set; } = string.Empty;
    
    public List<string> Users { get; set; } = new List<string>();
}

// UserRoleViewModel.cs
public class UserRoleViewModel
{
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public bool IsSelected { get; set; }
}