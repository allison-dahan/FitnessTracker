using System;
using System.Linq.Expressions;
using FitnessTracker.Models;
using FitnessTracker.Services;
using FitnessTracker.Repositories.Interfaces;

namespace FitnessTracker.Repositories.Implementations
{
    /// <summary>
    /// Wrapper for Profile Repository that implements both specific and generic interfaces
    /// </summary>
    public class ProfileRepositoryWrapper : IProfileRepository, IGenericRepository<UserProfile>
    {
        private readonly MockProfileRepository _mockRepository;
        private List<UserProfile> _entities;

        public ProfileRepositoryWrapper(MockProfileRepository mockRepository)
        {
            _mockRepository = mockRepository ?? throw new ArgumentNullException(nameof(mockRepository));
            
            var profile = _mockRepository.GetUserProfile();
            _entities = profile != null ? new List<UserProfile> { profile } : new List<UserProfile>();
        }

        // IGenericRepository<UserProfile> Implementation
        public IEnumerable<UserProfile> GetAll() => _entities;

        public IEnumerable<UserProfile> Find(Expression<Func<UserProfile, bool>> predicate)
        {
            return _entities.AsQueryable().Where(predicate).ToList();
        }

        public UserProfile GetById(int id)
        {
            return _entities.FirstOrDefault(p => p.Id == id);
        }

        public void Add(UserProfile entity)
        {
            _entities.Add(entity);
        }

        public void Update(UserProfile entity)
        {
            // Update via mock repository to maintain consistency
            _mockRepository.UpdateProfile(entity);
            _entities = new List<UserProfile> { _mockRepository.GetUserProfile() };
        }

        public void Delete(int id)
        {
            var profile = _entities.FirstOrDefault(p => p.Id == id);
            if (profile != null)
            {
                _entities.Remove(profile);
            }
        }

        // IProfileRepository Implementation
        public UserProfile GetUserProfile() => _mockRepository.GetUserProfile();
        public void UpdateProfile(UserProfile profile) => _mockRepository.UpdateProfile(profile);
        public void UpdateGoals(UserProfile profile) => _mockRepository.UpdateGoals(profile);
    }
}