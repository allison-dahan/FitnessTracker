using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Models;
using FitnessTracker.DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
namespace FitnessTracker.Controllers;

[Authorize]
public class WorkoutsController : Controller
{
    private readonly IWorkoutRepository _repository;

    public WorkoutsController(IWorkoutRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Index()
    {
        var workouts = _repository.GetAllWorkouts();
        return View(workouts);
    }

    [AllowAnonymous]
    public IActionResult GetRecentWorkouts()
    {
        var workouts = _repository.GetAllWorkouts();
        return PartialView("_RecentWorkouts", workouts);
    }

    [AllowAnonymous]
    public IActionResult GetWorkoutStatistics()
    {

        return ViewComponent("WorkoutStatistics");
    }

    public IActionResult Details(int id)
    {
        var workout = _repository.GetWorkoutById(id);
        ViewData["PageTitle"] = $"Workout on {workout.Date.ToShortDateString()}";
        ViewData["Workout"] = workout;
        return View();
    }
    
    public IActionResult Edit(int id)
    {
        var workout = _repository.GetWorkoutById(id);
        ViewBag.PageTitle = $"Edit Workout - {workout.Date.ToShortDateString()}";
        return View(workout);
    }

    [HttpPost]
    public IActionResult Edit(Workout workout)
    {
        if (ModelState.IsValid)
        {
            _repository.UpdateWorkout(workout);
            return RedirectToAction(nameof(Index));
        }
        return View(workout);
    }

    // Changed to return Workout instead of WorkoutViewModel
    public IActionResult Create()
    {
        // Use ViewBag for dropdown data
        ViewBag.AvailableExercises = _repository.GetAllExercises().ToList();
        ViewBag.WorkoutTypes = new List<string> { 
            "Strength Training", "Cardio", "HIIT", 
            "Flexibility", "Mixed" 
        };
        
        // Return a new Workout
        return View(new Workout { Date = DateTime.Now });
    }

    // Changed to accept Workout instead of WorkoutViewModel
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Workout workout)
    {
        if (ModelState.IsValid)
        {
            _repository.AddWorkout(workout);
            return RedirectToAction(nameof(Index));
        }

        // Repopulate dropdown data on validation error
        ViewBag.AvailableExercises = _repository.GetAllExercises().ToList();
        ViewBag.WorkoutTypes = new List<string> { 
            "Strength Training", "Cardio", "HIIT", 
            "Flexibility", "Mixed" 
        };
        return View(workout);
    }
}