namespace FitnessTracker.Services;
using FitnessTracker.Models;

public interface IStatsRepository
{
    DailyStatsModel GetDailyStats();
    IEnumerable<WeeklyStatsModel> GetWeeklyStats();
    UserGoalsModel GetUserGoals();
}