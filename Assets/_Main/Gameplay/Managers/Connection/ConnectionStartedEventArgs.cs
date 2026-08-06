using Main.Gameplay.Connections;

namespace Main.Gameplay.Managers.Connection
{
    public record ConnectionStartedEventArgs(IConnectionManager Manager, IConnection Connection);
}
