using Main.Gameplay.Connectors;

namespace Main.Gameplay.Connections
{
    public interface IConnection
    {
        void UpdatePoints();
        bool TryConnect(IConnectorSocket socketA, IConnectorSocket socketB, out ConnectionFailedReason failed);
        void Disconnect();
    }
}
