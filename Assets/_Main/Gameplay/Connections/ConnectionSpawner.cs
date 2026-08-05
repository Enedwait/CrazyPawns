using Zenject;

namespace Main.Gameplay.Connections
{
    public class ConnectionSpawner : IInitializable
    {
        private ConnectionPool _pool;

        public ConnectionSpawner(ConnectionPool pool)
        {
            this._pool = pool;
        }

        public void Initialize()
        { }

        public Connection Spawn()
        {
            Connection connection = _pool.Spawn();
            connection.OnSpawned(_pool);
            return connection;
        }
    }
}
