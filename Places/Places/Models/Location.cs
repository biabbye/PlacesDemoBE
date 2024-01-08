namespace Places.Models
{
    public class Location
    {
        public Location()
        {
            UserProfiles = new HashSet<UserProfile>();
            Events = new HashSet<Event>();
        }
        public int Id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }

        public ICollection<UserProfile> UserProfiles { get; set; }
        public ICollection<Event> Events { get; set; }

    }
}
