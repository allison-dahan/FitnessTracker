using System;
using FitnessTracker.Models;
using FitnessTracker.Services;

namespace FitnessTracker.Repositories.Implementations
{
    /// <summary>
    /// Wrapper for Stats Repository 
    /// </summary>
    public class StatsRepositoryWrapper : IStatsRepository
    {
        private readonly MockStatsRepository _mockRepository;

        public StatsRepositoryWrapper(MockStatsRepository mockRepository)
        {
            _mockRepository = mockRepository ?? throw new ArgumentNullException(nameof(mockRepository));
        }

        // IStatsRepository Implementation
        public DailyStatsModel GetDailyStats() => _mockRepository.GetDailyStats();
        public IEnumerable<WeeklyStatsModel> GetWeeklyStats() => _mockRepository.GetWeeklyStats();
        public UserGoalsModel GetUserGoals() => _mockRepository.GetUserGoals();
    }
}