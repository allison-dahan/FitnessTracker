using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Models;
using FitnessTracker.Services;

namespace FitnessTracker.Controllers
{
    public class NutritionController : Controller
    {
        private readonly INutritionRepository _nutritionRepository;

        public NutritionController(INutritionRepository nutritionRepository)
        {
            _nutritionRepository = nutritionRepository;
        }

        // Main nutrition dashboard
        public IActionResult Index()
        {
            var viewModel = new NutritionViewModel
            {
                Summary = _nutritionRepository.GetTodaysSummary(),
                Meals = _nutritionRepository.GetTodaysMeals()
            };
            
            return View(viewModel);
        }

        // Get nutrition data for a specific date
        [HttpGet]
        public IActionResult ByDate(DateTime date)
        {
            var viewModel = new NutritionViewModel
            {
                Summary = _nutritionRepository.GetSummaryByDate(date),
                Meals = _nutritionRepository.GetMealsByDate(date),
                SelectedDate = date
            };
            
            return View("Index", viewModel);
        }

        // Display form to log a new meal
        [HttpGet]
        public IActionResult LogMeal()
        {
            var meal = new Meal
            {
                Date = DateTime.Now.Date
            };
            
            ViewBag.MealTypes = new List<string> { "Breakfast", "Lunch", "Dinner", "Snack" };
            
            return View(meal);
        }

        // Process the submitted meal log form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogMeal(Meal meal)
        {
            if (ModelState.IsValid)
            {
                _nutritionRepository.AddMeal(meal);
                return RedirectToAction(nameof(Index));
            }
            
            return View(meal);
        }

        // Display form to edit an existing meal
        [HttpGet]
        public IActionResult EditMeal(int id)
        {
            var meal = _nutritionRepository.GetAllMeals().FirstOrDefault(m => m.Id == id);
            
            if (meal == null)
            {
                return NotFound();
            }
            
            return View(meal);
        }

        // Process the submitted meal edit form
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditMeal(Meal meal)
        {
            if (ModelState.IsValid)
            {
                _nutritionRepository.UpdateMeal(meal);
                return RedirectToAction(nameof(Index));
            }
            
            return View(meal);
        }

        // Confirm deletion of a meal
        [HttpGet]
        public IActionResult DeleteMeal(int id)
        {
            var meal = _nutritionRepository.GetAllMeals().FirstOrDefault(m => m.Id == id);
            
            if (meal == null)
            {
                return NotFound();
            }
            
            return View(meal);
        }

        // Process the confirmed meal deletion
        [HttpPost, ActionName("DeleteMeal")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMealConfirmed(int id)
        {
            _nutritionRepository.DeleteMeal(id);
            return RedirectToAction(nameof(Index));
        }

        // Log water intake
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogWater(WaterIntake waterIntake)
        {
            if (ModelState.IsValid)
            {
                waterIntake.Date = DateTime.Now.Date;
                _nutritionRepository.AddWaterIntake(waterIntake);
                return RedirectToAction(nameof(Index));
            }
            
            // If there's an error, redirect back to index with error message
            TempData["ErrorMessage"] = "Please enter a valid amount of water (0.1L - 2.0L).";
            return RedirectToAction(nameof(Index));
        }

        // Display nutrition history/reports
        public IActionResult History()
        {
            var meals = _nutritionRepository.GetAllMeals();
            return View(meals);
        }

        // AJAX endpoint for adding water quickly
        [HttpPost]
        public IActionResult QuickAddWater(decimal amount)
        {
            if (amount > 0 && amount <= 2.0m)
            {
                var waterIntake = new WaterIntake
                {
                    Date = DateTime.Now.Date,
                    Amount = amount,
                    UserId = 1
                };
                
                _nutritionRepository.AddWaterIntake(waterIntake);
                
                // For AJAX requests, return partial result
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    var newTotal = _nutritionRepository.GetTodaysWaterIntake();
                    var goal = _nutritionRepository.GetTodaysSummary().WaterGoal;
                    
                    return Json(new { 
                        success = true, 
                        newTotal = newTotal,
                        goal = goal,
                        percentage = Math.Min((newTotal / goal * 100), 100)
                    });
                }
            }
            
            return RedirectToAction(nameof(Index));
        }

    }

    // View Model to combine nutrition data for the view
    public class NutritionViewModel
    {
        public NutritionSummaryModel Summary { get; set; }
        public IEnumerable<Meal> Meals { get; set; }
        public DateTime SelectedDate { get; set; } = DateTime.Now.Date;
        public WaterIntakeModel NewWaterIntake { get; set; } = new WaterIntakeModel();
    }
}