using Places.Models;

namespace Places.Interfaces
{
    public interface IConnectionRepository
    {
        ICollection<Connection> GetConnections();
        Connection GetConnection(int connectionId);
        ICollection<Connection> GetConnectionOfAUser(int userProfileId);
        ICollection<Connection> GetPendingConnections(int userProfileId);

        bool CheckConnectionBetweenUsers(int senderId, int receiverId);
        bool ConnectionExists(int connectionId);
        bool CreateConnection(Connection connection);
        bool UpdateConnection(Connection connection);
        bool DeleteConnection(Connection connection);
        bool Save();
    }
}
