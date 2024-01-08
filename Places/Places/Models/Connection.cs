namespace Places.Models
{
    public class Connection
    {

        public int Id { get; set; }
        public int? SenderId { get; set; }
        public UserProfile Sender { get; set; }
        public int? ReceiverId { get; set; }
        public UserProfile Receiver { get; set; }
        public ConnectionStatus Status { get; set; }

        public enum ConnectionStatus
        {
            Declined,
            Pending,
            Accepted,
            Blocked
        }

    }
}
