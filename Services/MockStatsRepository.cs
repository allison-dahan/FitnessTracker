using FitnessTracker.Models;

namespace FitnessTracker.Services;

public class MockStatsRepository : IStatsRepository
{
    public DailyStatsModel GetDailyStats()
    {
        return new DailyStatsModel
        {
            CaloriesConsumed = 1850,
            CaloriesGoal = 2000,
            WorkoutsThisWeek = 4,
            WorkoutsGoal = 5,
            WaterIntake = 1.8m,
            WaterGoal = 2.5m,
            StepsToday = 7532,
            StepsGoal = 1000
        };
    }

    public IEnumerable<WeeklyStatsModel> GetWeeklyStats()
    {
        return new List<WeeklyStatsModel>
        {
            new WeeklyStatsModel {Day = "Monday", Calories = 1950, Steps = 8500 },
            new WeeklyStatsModel {Day = "Tuesday", Calories = 1850, Steps = 7200 },
            new WeeklyStatsModel {Day = "Wednesday", Calories = 2100, Steps = 9800 },
            new WeeklyStatsModel {Day = "Thursday", Calories = 1750, Steps = 6500 },
            new WeeklyStatsModel {Day = "Friday", Calories = 2200, Steps = 10200 },
            new WeeklyStatsModel {Day = "Saturday", Calories = 1900, Steps = 8700 },
            new WeeklyStatsModel {Day = "Sunday", Calories = 1650, Steps = 5600 },

        };
    }

    public UserGoalsModel GetUserGoals()
    {
        return new UserGoalsModel
        {
            WeightGoal = 75.0m,
            CaloriesPerDay = 2000,
            WorkoutsPerWeek = 5,
            StepsPerDay = 10000

        };
    }
}