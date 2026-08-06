using Main.Common.Classes.Registries;

namespace Main.Gameplay.Connectors
{
    public class ConnectorRegistry : Registry<Connector>, IConnectorRegistry
    {
        #region Init

        public ConnectorRegistry(int capacity) : base(capacity)
        { }

        #endregion
    }

    public interface IConnectorRegistry : IRegistry<Connector>
    { }
}
