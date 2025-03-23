using FitnessTracker.DataAccess.Interfaces;
using FitnessTracker.Models;

namespace FitnessTracker.DataAccess.Repositories;
public class ProfileDatabaseRepository : IProfileRepository
{
    private readonly FitnessTrackerContext _context; // Your database context

    public ProfileDatabaseRepository(FitnessTrackerContext context)
    {
        _context = context;
    }

    public UserProfile GetUserProfile()
{
    // Ensure you have a default profile if no profile exists
    var profile = _context.UserProfiles.FirstOrDefault();
    
    if (profile == null)
    {
        // Create a default profile if none exists
        profile = new UserProfile
        {
            FirstName = "Default",
            LastName = "User",
            Email = "defaul@example.com",
            Age = 25,
            Gender = "Not Specified",
            Height = 170,
            Weight = 70,
            FitnessGoal = "Get Fit",
            TargetWeight = 68
        };
        
        _context.UserProfiles.Add(profile);
        _context.SaveChanges();
    }
    
    return profile;
}



    public void UpdateProfile(UserProfile profile)
    {
        var existingProfile = _context.UserProfiles
            .FirstOrDefault(p => p.Id == profile.Id);

        if (existingProfile != null)
        {
            // Update database record
            existingProfile.FirstName = profile.FirstName;
            existingProfile.LastName = profile.LastName;
            existingProfile.Email = profile.Email;
            existingProfile.Age = profile.Age;
            existingProfile.Gender = profile.Gender;
            existingProfile.Height = profile.Height;
            existingProfile.Weight = profile.Weight;

            _context.SaveChanges();
        }
    }

    public void UpdateGoals(UserProfile profile)
    {
        var existingProfile = _context.UserProfiles
            .FirstOrDefault(p => p.Id == profile.Id);

        if (existingProfile != null)
        {
            existingProfile.FitnessGoal = profile.FitnessGoal;
            existingProfile.TargetWeight = profile.TargetWeight;

            _context.SaveChanges();
        }
    }
}