using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Models.Identity;
using FitnessTracker.Web.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace FitnessTracker.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear existing external cookie
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            
            // Populate external providers
            ViewData["ExternalLogins"] = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return RedirectToLocal(returnUrl);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser 
                { 
                    UserName = model.Email, 
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Gender = model.Gender,
                    City = model.City,
                    PostalCode = model.PostalCode,
                    DateOfBirth = model.DateOfBirth
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    
                    // Create and link UserProfile if needed
                    // var userProfile = new UserProfile { IdentityUserId = user.Id, /* other properties */ };
                    // _context.UserProfiles.Add(userProfile);
                    // await _context.SaveChangesAsync();
                    
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation("User signed in after registration.");
                    return RedirectToLocal(returnUrl);
                }
                
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Lockout()
        {
            return View();
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        


[HttpPost]
[AllowAnonymous]
[ValidateAntiForgeryToken]
public IActionResult ExternalLogin(string provider, string returnUrl = null)
{
    // Request a redirect to the external login provider
    var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { returnUrl });
    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
    return Challenge(properties, provider);
}

[HttpGet]
[AllowAnonymous]
public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
{
    returnUrl = returnUrl ?? Url.Content("~/");

    // Handle errors from external provider
    if (remoteError != null)
    {
        _logger.LogError($"Error from external provider: {remoteError}");
        ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
        return RedirectToAction(nameof(Login));
    }

    // Get the login info from the external provider
    var info = await _signInManager.GetExternalLoginInfoAsync();
    if (info == null)
    {
        _logger.LogWarning("Could not get external login info");
        return RedirectToAction(nameof(Login));
    }

    // Sign in the user with the external login provider if they already have an account
    var signInResult = await _signInManager.ExternalLoginSignInAsync(
        info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

    if (signInResult.Succeeded)
    {
        _logger.LogInformation("{Name} logged in with {LoginProvider} provider", 
            info.Principal.Identity.Name, info.LoginProvider);
        return RedirectToLocal(returnUrl);
    }

    if (signInResult.IsLockedOut)
    {
        _logger.LogWarning("User account locked out");
        return RedirectToAction(nameof(Lockout));
    }
    
    // If user doesn't have an account, extract info from the external provider and show confirmation form
    var email = info.Principal.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
    var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName) ?? string.Empty;
    var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname) ?? string.Empty;
    
    // Show the external login confirmation page
    return View("ExternalLoginConfirmation", new ExternalLoginViewModel 
    {
        Email = email,
        FirstName = firstName,
        LastName = lastName,
        ProviderDisplayName = info.ProviderDisplayName ?? "External Provider"
    });
}

[HttpPost]
[AllowAnonymous]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginViewModel model, string returnUrl = null)
{
    returnUrl = returnUrl ?? Url.Content("~/");

    if (ModelState.IsValid)
    {
        // Get the external login info again
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            _logger.LogError("Error loading external login information during confirmation");
            return View("Error");
        }

        // Create a new user with the provided email
        var user = new ApplicationUser 
        { 
            UserName = model.Email, 
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            EmailConfirmed = true // Since the email is verified by the external provider
        };

        // Try to create the user
        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
            // Add user to the User role
            await _userManager.AddToRoleAsync(user, "User");
            
            // Add the external login to the user
            result = await _userManager.AddLoginAsync(user, info);
            if (result.Succeeded)
            {
                // Sign in the user
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("User created an account using {Name} provider", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
        }

        // If there were any errors, add them to the ModelState
        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }
    }

    // If we got this far, something failed, redisplay form
    ViewData["ReturnUrl"] = returnUrl;
    return View(model);
}
    }
}