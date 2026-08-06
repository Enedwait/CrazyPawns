using Main.Gameplay.Connections;

namespace Main.Gameplay.Managers.Connection
{
    public record ConnectionEstablishedEventArgs(IConnectionManager Manager, IConnection Connection);
}
