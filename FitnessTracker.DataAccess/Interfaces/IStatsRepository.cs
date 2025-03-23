using FitnessTracker.Models;
namespace FitnessTracker.DataAccess.Interfaces;

public interface IStatsRepository
{
    DailyStatsModel GetDailyStats();
    IEnumerable<WeeklyStatsModel> GetWeeklyStats();
    UserGoalsModel GetUserGoals();
}