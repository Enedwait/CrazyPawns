using Main.Gameplay.Connectors;
using UnityEngine.Events;

namespace Main.Gameplay.Managers.Connection
{
    public interface IConnectionManager : IManager
    {
        event UnityAction<ConnectionStartedEventArgs> onConnectionStarted;
        event UnityAction<ConnectionEndedEventArgs> onConnectionEnded;
        event UnityAction<ConnectionEstablishedEventArgs> onConnectionEstablished;

        void BeginConnect(IConnectorSocket from);
        void EndConnect();
    }
}
