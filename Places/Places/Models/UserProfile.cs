namespace Places.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Username { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? City { get; set; }
        public string? Interest { get; set; }
        public string? ImageUrl { get; set; }
    }
}
