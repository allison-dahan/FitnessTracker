using System.ComponentModel.DataAnnotations;
namespace FitnessTracker.Web.Models;
// RoleViewModel.cs
public class RoleViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
}

// CreateRoleViewModel.cs
public class CreateRoleViewModel
{
    [Required]
    [Display(Name = "Role Name")]
    public string RoleName { get; set; }
}

// EditRoleViewModel.cs
public class EditRoleViewModel
{
    public string Id { get; set; }
    
    [Required(ErrorMessage = "Role Name is required")]
    public string RoleName { get; set; }
    
    public List<string> Users { get; set; } = new List<string>();
}

// UserRoleViewModel.cs
public class UserRoleViewModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public bool IsSelected { get; set; }
}