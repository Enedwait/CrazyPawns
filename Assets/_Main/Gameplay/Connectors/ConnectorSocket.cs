using System.Collections.Generic;
using Main.Common.Extensions;
using Main.Gameplay.Connections;
using UnityEngine;

namespace Main.Gameplay.Connectors
{
    [DisallowMultipleComponent]
    public class ConnectorSocket : MonoBehaviour, IConnectorSocket
    {
        #region Fields

        [SerializeField] private Transform root;

        private List<IConnection> _connections = new List<IConnection>();

        #endregion

        #region Properties

        public Transform Root => root;
        public Vector3 Position => transform != null ? transform.position : Vector3.zero;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (root == null)
                root = transform;
        }

        private void OnDisable()
        {
            DisconnectAll();
        }

        #endregion

        #region Connect

        public void Connect(IConnection connection)
        {
            if (connection.IsNullOrDestroyed()) return;
            if (_connections.Contains(connection)) return;

            _connections.Add(connection);
        }

        #endregion

        #region Disconnect

        public void Disconnect(IConnection connection)
        {
            if (connection == null) return;
            if (!_connections.Contains(connection)) return;

            _connections.Remove(connection);
            connection.Disconnect();
        }

        public void DisconnectAll()
        {
            for(int i = _connections.Count - 1;  i >= 0; i--)
                Disconnect(_connections[i]);
        }

        #endregion
    }
}
