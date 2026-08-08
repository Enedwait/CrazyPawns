namespace Main.Gameplay.Connections
{
    public sealed class ConnectionSettingsProvider : IConnectionSettingsProvider
    {
        #region Fields

        private ConnectionSettings _settings;

        #endregion

        #region Init

        public ConnectionSettingsProvider(ConnectionSettings settings)
        {
            SetSettings(settings);
        }

        #endregion

        #region Methods

        public ConnectionSettings GetSettings() => 
            _settings;

        public void SetSettings(ConnectionSettings settings) => 
            this._settings = settings;

        #endregion
    }
}
