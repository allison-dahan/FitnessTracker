using FitnessTracker.DataAccess.Interfaces;
using FitnessTracker.Models;

namespace FitnessTracker.DataAccess.Repositories
{
    public class StatsDatabaseRepository : IStatsRepository
    {
        private readonly FitnessTrackerContext _context;

        public StatsDatabaseRepository(FitnessTrackerContext context)
        {
            _context = context;
        }

        public DailyStatsModel GetDailyStats()
        {
            // This would typically pull from various tables
            // For now, a basic implementation
            return new DailyStatsModel
            {
                CaloriesConsumed = _context.Meals
                    .Where(m => m.Date.Date == DateTime.Now.Date)
                    .Sum(m => m.Calories),
                CaloriesGoal = 2000,
                WorkoutsThisWeek = _context.Workouts
                    .Count(w => w.Date >= DateTime.Now.AddDays(-7)),
                WorkoutsGoal = 5,
                WaterIntake = _context.WaterIntakes
                    .Where(w => w.Date.Date == DateTime.Now.Date)
                    .Sum(w => w.Amount),
                WaterGoal = 2.5m,
                StepsToday = 7532, // This would need a separate tracking mechanism
                StepsGoal = 10000
            };
        }

        public IEnumerable<WeeklyStatsModel> GetWeeklyStats()
        {
            // This would typically be more complex with actual database queries
            var startOfWeek = DateTime.Now.AddDays(-7);
            return _context.Meals
                .Where(m => m.Date >= startOfWeek)
                .GroupBy(m => m.Date.DayOfWeek)
                .Select(g => new WeeklyStatsModel
                {
                    Day = g.Key.ToString(),
                    Calories = g.Sum(m => m.Calories),
                    Steps = 7500 // Placeholder - would need actual steps tracking
                })
                .ToList();
        }

        public UserGoalsModel GetUserGoals()
        {
            // This might come from a user profile or goals table
            return new UserGoalsModel
            {
                WeightGoal = 75.0m,
                CaloriesPerDay = 2000,
                WorkoutsPerWeek = 5,
                StepsPerDay = 10000
            };
        }
    }
}