using Main.Gameplay.Connectors;
using Vector3 = UnityEngine.Vector3;

namespace Main.Gameplay.Connections
{
    public interface IConnection
    {
        void MoveStartAt(Vector3 position);
        void MoveEndAt(Vector3 position);
        void UpdatePoints();
        bool TryConnect(IConnectorSocket socketA, IConnectorSocket socketB, out ConnectionFailedReason failed);
        void Disconnect();
    }
}
