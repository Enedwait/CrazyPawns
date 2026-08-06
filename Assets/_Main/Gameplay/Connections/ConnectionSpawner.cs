using Zenject;

namespace Main.Gameplay.Connections
{
    public class ConnectionSpawner : IInitializable
    {
        #region Fields

        private ConnectionPool _pool;

        #endregion

        #region Init

        public ConnectionSpawner(ConnectionPool pool)
        {
            this._pool = pool;
        }

        public void Initialize()
        { }

        #endregion

        #region Spawn

        public Connection Spawn()
        {
            Connection connection = _pool.Spawn();
            connection.OnSpawned(_pool);
            return connection;
        }

        #endregion
    }
}
