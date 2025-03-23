using FitnessTracker.Business.DTOs;

namespace FitnessTracker.Business.Services
{
    public interface IProfileService
    {
        UserProfileDto GetUserProfile();
        void UpdateProfile(UserProfileDto profile);
        void UpdateFitnessGoals(UserProfileDto profile);
    }
}