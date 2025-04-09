// In ViewComponents/WorkoutStatisticsViewComponent.cs
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.DataAccess.Interfaces;

namespace FitnessTracker.Web.ViewComponents
{
    public class WorkoutStatisticsViewComponent : ViewComponent
    {
        private readonly IWorkoutRepository _repository;

        public WorkoutStatisticsViewComponent(IWorkoutRepository repository)
        {
            _repository = repository;
        }

        public IViewComponentResult Invoke()
        {
            var workouts = _repository.GetAllWorkouts();
            
            var stats = new WorkoutStatsViewModel
            {
                TotalWorkouts = workouts.Count(),
                TotalMinutes = workouts.Sum(w => w.Duration),
                // Add additional statistics as needed
            };
            
            return View(stats);
        }
    }
    
    public class WorkoutStatsViewModel
    {
        public int TotalWorkouts { get; set; }
        public int TotalMinutes { get; set; }
        public double AverageCalories { get; set; }
    }
}