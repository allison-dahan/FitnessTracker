using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Business.Services;
using FitnessTracker.Business.DTOs;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Authorization;


namespace FitnessTracker.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        // GET: Profile
        public IActionResult Index()
        {
            // Convert DTO to Model
        var profileDto = _profileService.GetUserProfile();
        var profileModel = new UserProfile
        {
            Id = profileDto.Id,
            FirstName = profileDto.FirstName,
            LastName = profileDto.LastName,
            Email = profileDto.Email,
            Age = profileDto.Age,
            Gender = profileDto.Gender,
            Height = profileDto.Height,
            Weight = profileDto.Weight,
            FitnessGoal = profileDto.FitnessGoal,
            TargetWeight = profileDto.TargetWeight
        };

        return View(profileModel);
    }

        // GET: Profile/Edit
        public IActionResult Edit()
        {
            var profileDto = _profileService.GetUserProfile();
            var profileModel = new UserProfile
    {
        Id = profileDto.Id,
        FirstName = profileDto.FirstName,
        LastName = profileDto.LastName,
        Email = profileDto.Email,
        Age = profileDto.Age,
        Gender = profileDto.Gender,
        Height = profileDto.Height,
        Weight = profileDto.Weight,
        FitnessGoal = profileDto.FitnessGoal,
        TargetWeight = profileDto.TargetWeight
    };
    return View(profileModel);
        }

        // POST: Profile/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserProfile userProfile)
        {
            ModelState.Remove(nameof(userProfile.TargetWeight));
            if (ModelState.IsValid)
    {
        // Convert UserProfile to UserProfileDto
        var profileDto = new UserProfileDto
        {
            Id = userProfile.Id,
            FirstName = userProfile.FirstName,
            LastName = userProfile.LastName,
            Email = userProfile.Email,
            Age = userProfile.Age,
            Gender = userProfile.Gender,
            Height = userProfile.Height,
            Weight = userProfile.Weight
        };

        _profileService.UpdateProfile(profileDto);
        return RedirectToAction(nameof(Index));
    }

     // If model is not valid, return to Index view
    return View(nameof(Index), userProfile);
        }

        // POST: Profile/UpdateGoals
        [HttpPost]
[ValidateAntiForgeryToken]
public IActionResult UpdateGoals(UserProfile userProfile)
{
    // Remove validation for personal info fields
    ModelState.Remove(nameof(userProfile.FirstName));
    ModelState.Remove(nameof(userProfile.LastName));
    ModelState.Remove(nameof(userProfile.Email));
    ModelState.Remove(nameof(userProfile.Age));
    ModelState.Remove(nameof(userProfile.Gender));
    ModelState.Remove(nameof(userProfile.Height));
    ModelState.Remove(nameof(userProfile.Weight));

    if (ModelState.IsValid)
    {
        var profileDto = new UserProfileDto
        {
            Id = userProfile.Id,
            FitnessGoal = userProfile.FitnessGoal,
            TargetWeight = userProfile.TargetWeight
        };

        _profileService.UpdateFitnessGoals(profileDto);
        return RedirectToAction(nameof(Index));
    }

    // If model is not valid, return to Index view
    return View(nameof(Index), userProfile);
}
    }
}