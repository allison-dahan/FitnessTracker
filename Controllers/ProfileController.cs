using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Models;
using FitnessTracker.Services;

namespace FitnessTracker.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileController(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        // GET: Profile
        public IActionResult Index()
        {
            var profile = _profileRepository.GetUserProfile();
            return View(profile);
        }

        // POST: Profile/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(UserProfile profile)
        {
            if (ModelState.IsValid)
            {
                _profileRepository.UpdateProfile(profile);
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View("Index", profile);
        }

        // POST: Profile/UpdateGoals
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateGoals(UserProfile profile)
        {
            if (ModelState.IsValid)
            {
                _profileRepository.UpdateGoals(profile);
                TempData["SuccessMessage"] = "Fitness goals updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View("Index", profile);
        }
    }
}