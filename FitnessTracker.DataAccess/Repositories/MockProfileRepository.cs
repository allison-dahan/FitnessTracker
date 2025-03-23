using FitnessTracker.DataAccess.Interfaces;
using FitnessTracker.Models;

namespace FitnessTracker.DataAccess.Repositories;

public class MockProfileRepository : IProfileRepository
    {
        private UserProfile _userProfile;

        public MockProfileRepository()
        {
            // Initialize with mock data
            _userProfile = new UserProfile
            {
                Id = 1,
                FirstName = "Allison",
                LastName = "Dd",
                Email = "ad@gmail.com",
                Age = 23,
                Gender = "Female",
                Height = 154,
                Weight = 54,
                FitnessGoal = "Muscle Gain",
                TargetWeight = 56
            };
        }

        public UserProfile GetUserProfile()
        {
            return _userProfile;
        }

        public void UpdateProfile(UserProfile profile)
        {
            // In a real app, this would update the database
            _userProfile.FirstName = profile.FirstName;
            _userProfile.LastName = profile.LastName;
            _userProfile.Email = profile.Email;
            _userProfile.Age = profile.Age;
            _userProfile.Gender = profile.Gender;
            _userProfile.Height = profile.Height;
            _userProfile.Weight = profile.Weight;
        }

        public void UpdateGoals(UserProfile profile)
        {
            // Update just the fitness goals
            _userProfile.FitnessGoal = profile.FitnessGoal;
            _userProfile.TargetWeight = profile.TargetWeight;
        }
    }