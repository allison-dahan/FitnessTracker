using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Business.Services;
using FitnessTracker.Business.DTOs;

namespace FitnessTracker.Web.Controllers
{
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
            var profileDto = _profileService.GetUserProfile();
            return View(profileDto);
        }

        // GET: Profile/Edit
        public IActionResult Edit()
        {
            var profileDto = _profileService.GetUserProfile();
            return View(profileDto);
        }

        // POST: Profile/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserProfileDto profileDto)
        {
            if (ModelState.IsValid)
            {
                _profileService.UpdateProfile(profileDto);
                return RedirectToAction(nameof(Index));
            }
            return View(profileDto);
        }

        // POST: Profile/UpdateGoals
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateGoals(UserProfileDto profileDto)
        {
            if (ModelState.IsValid)
            {
                _profileService.UpdateFitnessGoals(profileDto);
                return RedirectToAction(nameof(Index));
            }
            return View(nameof(Index), profileDto);
        }
    }
}