using Main.Gameplay.Connections;
using Zenject;

namespace Main.Gameplay.Managers
{
    public class ConnectionUpdateManager : ITickable
    {
        private ConnectionPool _connectionPool;

        public ConnectionUpdateManager(ConnectionPool connectionPool)
        {
            this._connectionPool = connectionPool;
        }

        public void Tick()
        {
            foreach (var connection in _connectionPool.ActiveItems)
                connection.UpdatePoints();
        }
    }
}
