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
            var endRes = DateTime.Now.Date;
            var startRes = endRes.AddDays(-6);

            // Fetch data from DB
            var meals = _context.Meals
                .Where(m => m.Date >= startRes && m.Date <= endRes.AddDays(1).AddTicks(-1))
                .ToList();
                
            var workouts = _context.Workouts
                .Where(w => w.Date >= startRes && w.Date <= endRes.AddDays(1).AddTicks(-1))
                .ToList();

            var stats = new List<WeeklyStatsModel>();

            for (int i = 0; i < 7; i++)
            {
                var date = startRes.AddDays(i);
                stats.Add(new WeeklyStatsModel
                {
                    Day = date.DayOfWeek.ToString(),
                    Calories = meals.Where(m => m.Date.Date == date).Sum(m => m.Calories),
                    WorkoutCount = workouts.Count(w => w.Date.Date == date),
                    Steps = 7500 // Placeholder
                });
            }

            return stats;
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