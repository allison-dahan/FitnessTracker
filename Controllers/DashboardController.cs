using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Models;
using FitnessTracker.Services;


namespace FitnessTracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IWorkoutRepository _workoutRepository;
        private readonly INutritionRepository _nutritionRepository;
        private readonly IStatsRepository _statsRepository;


        public DashboardController(IWorkoutRepository workoutRepository, INutritionRepository nutritionRepository, IStatsRepository statsRepository)
        {
            _workoutRepository = workoutRepository;
            _nutritionRepository = nutritionRepository;
            _statsRepository = statsRepository;

        } 

        public IActionResult Index() 
        {
            var dashboardViewModel = new DashboardViewModel
            {
                DailyStats = _statsRepository.GetDailyStats(),
                RecentWorkouts = _workoutRepository.GetRecentWorkouts(2),
                NutritionSummary = _nutritionRepository.GetTodaysSummary()

            };

            return View(dashboardViewModel);
        }

        public IActionResult WeeklyProgress()
        {
            var weeklyStats = _statsRepository.GetWeeklyStats();
            return View(weeklyStats);
        }

        public IActionResult Goals()
        {
            var goals = _statsRepository.GetUserGoals();
            return View(goals);
        }
    }
}