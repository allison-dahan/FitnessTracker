using FitnessTracker.Business.DTOs;
using FitnessTracker.DataAccess.Interfaces;
using FitnessTracker.Models;

namespace FitnessTracker.Business.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
        }

        public UserProfileDto GetUserProfile()
        {
            var userProfile = _profileRepository.GetUserProfile();
    
    if (userProfile == null)
    {
        // Return a default DTO if no profile found
        return new UserProfileDto
        {
            FirstName = "Default",
            LastName = "User",
            Age = 25,
            Gender = "Not Specified"
        };
    }
    
    return new UserProfileDto
    {
        Id = userProfile.Id,
        FirstName = userProfile.FirstName,
        LastName = userProfile.LastName,
        Email = userProfile.Email,
        Age = userProfile.Age,
        Gender = userProfile.Gender,
        Height = userProfile.Height,
        Weight = userProfile.Weight,
        FitnessGoal = userProfile.FitnessGoal,
        TargetWeight = userProfile.TargetWeight
    };  
        }

        public void UpdateProfile(UserProfileDto profileDto)
        {
            var userProfile = new UserProfile
            {
                Id = profileDto.Id,
                FirstName = profileDto.FirstName,
                LastName = profileDto.LastName,
                Email = profileDto.Email,
                Age = profileDto.Age,
                Gender = profileDto.Gender,
                Height = profileDto.Height,
                Weight = profileDto.Weight
            };

            _profileRepository.UpdateProfile(userProfile);
        }

        public void UpdateFitnessGoals(UserProfileDto profileDto)
        {
            var userProfile = new UserProfile
            {
                Id = profileDto.Id,
                FitnessGoal = profileDto.FitnessGoal,
                TargetWeight = profileDto.TargetWeight
            };

            _profileRepository.UpdateGoals(userProfile);
        }
    }
}