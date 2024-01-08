using Places.Models;

namespace Places.Interfaces
{
    public interface IUserProfileRepository
    {
        ICollection<UserProfile> GetUserProfiles();
        UserProfile GetUserProfile(int id);
        UserProfile GetUserProfileByPhone(string phoneNumber);
        //ICollection<UserProfile> GetConnectionsOfAUser(int userProfileId);
        bool UserProfileExists(int id);
        bool CreateUserProfile(UserProfile userProfile);
        bool UpdateUserProfile(UserProfile userProfile);
        bool DeleteUserProfile(UserProfile userProfile);
        bool Save();
    }
}
