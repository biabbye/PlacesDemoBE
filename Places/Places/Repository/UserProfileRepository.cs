using Microsoft.EntityFrameworkCore;
using Places.Data;
using Places.Interfaces;
using Places.Models;

namespace Places.Repository
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly PlacesContext _context;
        public UserProfileRepository(PlacesContext context)
        {
            _context = context;
        }

        public UserProfile GetUserProfile(int id)
        {
            return _context.UserProfile.Where(up => up.Id == id).FirstOrDefault();
        }

        public UserProfile GetUserProfileByPhone(string phoneNumber)
        {
            return _context.UserProfile.Where(up => up.PhoneNumber == phoneNumber).FirstOrDefault();
        }

        public ICollection<UserProfile> GetUserProfiles()
        {
            return _context.UserProfile.OrderBy(up => up.Id).ToList();
        }

        public bool UserProfileExists(int id)
        {
            return _context.UserProfile.Any(up => up.Id == id);
        }

        public bool CreateUserProfile(UserProfile userProfile)
        {
            
            _context.UserProfile.Add(userProfile);

            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateUserProfile(UserProfile userProfile)
        {
            _context.UserProfile.Update(userProfile);
            return Save();
        }

        public bool DeleteUserProfile(UserProfile userProfile)
        {
            _context.UserProfile.Remove(userProfile);
            return Save();
        }

        //public ICollection<UserProfile> GetConnectionsOfAUser(int userProfileId)
        //{
        //    return _context.Connections.Where(c => c.SenderId == userProfileId).Include(c => c.Receiver).ToList();
        //}
    }
}
