using Places.Models;
using static Places.Models.Connection;

namespace Places.Dto
{
    public class ConnectionDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public ConnectionStatus Status { get; set; }
        
    }
}
