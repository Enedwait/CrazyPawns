using Main.Common.Classes.Objects;
using Main.Gameplay.Connections;
using Zenject;

namespace Main.Gameplay.Managers
{
    public class ConnectionUpdateManager : DisposableObject, IManager, ITickable
    {
        #region Fields

        private ConnectionPool _connectionPool;

        #endregion

        #region Properties

        public bool IsActive { get; protected set; }

        #endregion

        #region Init

        public ConnectionUpdateManager(ConnectionPool connectionPool)
        {
            this._connectionPool = connectionPool;
        }

        #endregion

        #region Methods

        public void Tick()
        {
            if (!IsActive) return;

            foreach (var connection in _connectionPool.ActiveItems)
                connection.UpdatePoints();
        }
        
        public void SetActive(bool active)
        {
            IsActive = active;
        }

        protected override void DisposeManaged()
        {
            SetActive(false);
        }

        #endregion
    }
}
