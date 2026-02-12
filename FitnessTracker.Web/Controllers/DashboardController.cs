using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Authorization; 
using FitnessTracker.DataAccess.Interfaces; // Repository interfaces



namespace FitnessTracker.Controllers
{
    [Authorize]
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
            var weeklyStats = _statsRepository.GetWeeklyStats().ToList();
            
            var dashboardViewModel = new DashboardViewModel
            {
                DailyStats = _statsRepository.GetDailyStats(),
                RecentWorkouts = _workoutRepository.GetRecentWorkouts(2),
                NutritionSummary = _nutritionRepository.GetTodaysSummary(),
                
                WeeklyActivityChart = new ChartData 
                {
                    Labels = weeklyStats.Any(ws => ws.WorkoutCount > 0) 
                        ? weeklyStats.Select(ws => ws.Day).ToList()
                        : new List<string> { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" },
                    Data = weeklyStats.Any(ws => ws.WorkoutCount > 0)
                        ? weeklyStats.Select(ws => ws.WorkoutCount).ToList()
                        : new List<int> { 1, 0, 2, 1, 3, 0, 2 } // Mock data
                },
                
                WeeklyCaloriesChart = new ChartData
                {
                    Labels = weeklyStats.Any(ws => ws.Calories > 0)
                        ? weeklyStats.Select(ws => ws.Day).ToList()
                        : new List<string> { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" },
                    Data = weeklyStats.Any(ws => ws.Calories > 0)
                        ? weeklyStats.Select(ws => ws.Calories).ToList()
                        : new List<int> { 1800, 2100, 1950, 2200, 1850, 2300, 2000 }, // Mock data
                    GoalData = Enumerable.Repeat(2000, 7).ToList()
                }
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