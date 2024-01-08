using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Places.Data;
using Places.Interfaces;
using Places.Models;

namespace Places.Repository
{
    public class ConnectionRepository : IConnectionRepository
    {
        private readonly PlacesContext _context;
        private readonly IMapper _mapper;

        public ConnectionRepository(PlacesContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public bool CheckConnectionBetweenUsers(int senderId, int receiverId)
        {
            return _context.Connections.Any(c => (c.SenderId == senderId && c.ReceiverId == receiverId) || (c.SenderId == receiverId && c.ReceiverId == senderId));
        }

        public bool ConnectionExists(int connectionId)
        {
            return _context.Connections.Any(r => r.Id == connectionId);
        }

        public bool CreateConnection(Connection connection)
        {
            _context.Connections.Add(connection);
            return Save();
        }

        public bool DeleteConnection(Connection connection)
        {
            _context.Connections.Remove(connection);
            return Save();
        }

        public Connection GetConnection(int connectionId)
        {
            return _context.Connections.Where(c => c.Id == connectionId).FirstOrDefault();
        }


        public ICollection<Connection> GetConnectionOfAUser(int userProfileId)
        {
            var connections = _context.Connections.Where(c => (c.SenderId == userProfileId || c.ReceiverId == userProfileId) && c.Status == Connection.ConnectionStatus.Accepted).ToList();
            return connections;
        }

        public ICollection<Connection> GetConnections()
        {
            return _context.Connections.ToList();
        }

        public ICollection<Connection> GetPendingConnections(int userProfileId)
        {
            return _context.Connections.Where(c =>( c.SenderId == userProfileId || c.ReceiverId == userProfileId) && c.Status == Connection.ConnectionStatus.Pending).ToList();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateConnection(Connection connection)
        {
            _context.Connections.Update(connection);
            return Save();
        }
    }
}
