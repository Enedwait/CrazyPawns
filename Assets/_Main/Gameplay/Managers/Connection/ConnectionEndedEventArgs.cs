using Main.Gameplay.Connections;

namespace Main.Gameplay.Managers.Connection
{
    public record ConnectionEndedEventArgs(IConnectionManager Manager, IConnection Connection);
}
