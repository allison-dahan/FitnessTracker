using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace FitnessTracker.Web.Identity
{
    public class CustomPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : class
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            var errors = new List<IdentityError>();
            
            // Check for minimum length (already handled by default options, but we can double-check)
            if (password.Length < 8)
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordTooShort",
                    Description = "Password must be at least 8 characters long."
                });
            }
            
            // Check for uppercase letters
            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordRequiresUpper",
                    Description = "Password must contain at least one uppercase letter."
                });
            }
            
            // Check for lowercase letters
            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordRequiresLower",
                    Description = "Password must contain at least one lowercase letter."
                });
            }
            
            // Check for digits
            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordRequiresDigit",
                    Description = "Password must contain at least one digit."
                });
            }
            
            // Check for special characters
            if (!Regex.IsMatch(password, @"[^a-zA-Z0-9]"))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordRequiresNonAlphanumeric",
                    Description = "Password must contain at least one non-alphanumeric character."
                });
            }
            
            // Check for common passwords (add more as needed)
            var commonPasswords = new[] { "password", "12345678", "qwerty123", "letmein", "welcome1" };
            if (commonPasswords.Contains(password.ToLower()))
            {
                errors.Add(new IdentityError
                {
                    Code = "CommonPassword",
                    Description = "The password you chose is too common. Please choose a more unique password."
                });
            }
            
            // Check that password doesn't contain username
            var username = await manager.GetUserNameAsync(user);
            if (username != null && password.ToLower().Contains(username.ToLower()))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordContainsUserName",
                    Description = "Your password cannot contain your username."
                });
            }
            
            return errors.Count == 0 ? 
                IdentityResult.Success : 
                IdentityResult.Failed(errors.ToArray());
        }
    }
}