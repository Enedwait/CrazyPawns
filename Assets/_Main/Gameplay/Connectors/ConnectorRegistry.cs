using Main.Common.Classes.Registries;

namespace Main.Gameplay.Connectors
{
    public class ConnectorRegistry : Registry<Connector>
    {
        #region Init

        public ConnectorRegistry(int capacity) : base(capacity)
        { }

        #endregion
    }
}
