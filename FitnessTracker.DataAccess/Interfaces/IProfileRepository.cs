
using FitnessTracker.Models;
namespace FitnessTracker.DataAccess.Interfaces
{
    public interface IProfileRepository
    {
        UserProfile GetUserProfile();
        void UpdateProfile(UserProfile profile);
        void UpdateGoals(UserProfile profile);
    }
}