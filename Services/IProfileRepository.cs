using FitnessTracker.Models;

namespace FitnessTracker.Services
{
    public interface IProfileRepository
    {
        UserProfile GetUserProfile();
        void UpdateProfile(UserProfile profile);
        void UpdateGoals(UserProfile profile);
    }

    public class MockProfileRepository : IProfileRepository
    {
        private User _user;
        private UserProfile _userProfile;

        public MockProfileRepository()
        {
            // Initialize with mock data
            _user = new User
            {
                Id = 1,
                FirstName = "Allison",
                LastName = "Dd",
                Email = "ad@gmail.com",
                PasswordHash = "hashed_password",
                DateRegistered = DateTime.Now.AddYears(-1),
                LastLogin = DateTime.Now,
                IsActive = true
            };

            _userProfile = new UserProfile
            {
                Id = 1,
                UserId = 1,
                User = _user,
                Age = 23,
                Gender = "Female",
                Height = 154,
                Weight = 54,
                FitnessGoal = "Muscle Gain",
                TargetWeight = 56,
                ActivityLevel = "Moderate",
                DateUpdated = DateTime.Now
            };

            _user.Profile = _userProfile;
        }

        public UserProfile GetUserProfile()
        {
            return _userProfile;
        }

        public void UpdateProfile(UserProfile profile)
        {
            // Update the profile
            _userProfile.Age = profile.Age;
            _userProfile.Gender = profile.Gender;
            _userProfile.Height = profile.Height;
            _userProfile.Weight = profile.Weight;
            _userProfile.DateUpdated = DateTime.Now;
            
            // Also update the related User
            _user.FirstName = profile.User?.FirstName ?? _user.FirstName;
            _user.LastName = profile.User?.LastName ?? _user.LastName;
            _user.Email = profile.User?.Email ?? _user.Email;
        }

        public void UpdateGoals(UserProfile profile)
        {
            _userProfile.FitnessGoal = profile.FitnessGoal;
            _userProfile.TargetWeight = profile.TargetWeight;
            _userProfile.DateUpdated = DateTime.Now;
        }
    }
}