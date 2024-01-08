namespace Places.Models
{
    public class Event
    {
        public Event()
        {
            this.UserProfiles = new HashSet<UserProfile>();
        }
        public int Id { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string EventImage { get; set; }
        public DateTime EventTime { get; set; }
        public int EventLocationId { get; set; }
        public Location EventLocation { get; set; }
        public int MaxParticipants { get; set; }
        public EventType Type { get; set; }

        public virtual ICollection<UserProfile> UserProfiles { get; set; }

        public enum EventType
        {
            Declined,
            Pending,
            Accepted,
            Blocked
        }
    }
}
