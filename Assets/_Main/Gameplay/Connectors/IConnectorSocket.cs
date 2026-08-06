using Main.Gameplay.Connections;
using UnityEngine;

namespace Main.Gameplay.Connectors
{
    public interface IConnectorSocket
    {
        public Transform Root { get; }
        public Vector3 Position { get; }

        void Connect(IConnection connection);
        void Disconnect(IConnection connection);
        void DisconnectAll();
    }
}
