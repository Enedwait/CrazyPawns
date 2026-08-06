using System.Collections.Generic;
using Main.Gameplay.Connections;
using UnityEngine;

namespace Main.Gameplay.Connectors
{
    [DisallowMultipleComponent]
    public class ConnectorSocket : MonoBehaviour, IConnectorSocket
    {
        [SerializeField] private Transform root;

        private List<IConnection> _connections = new List<IConnection>();

        public Transform Root => root;
        public Vector3 Position => transform != null ? transform.position : Vector3.zero;

        private void Awake()
        {
            if (root == null)
                root = transform;
        }

        private void OnDisable()
        {
            DisconnectAll();
        }

        public void Connect(IConnection connection)
        {
            if (connection == null) return;
            if (_connections.Contains(connection)) return;

            _connections.Add(connection);
        }

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
    }
}
