namespace Main.Gameplay.Players
{
    public sealed class PlayerSpawner : IPlayerSpawner
    {
        #region Fields

        private IPlayerFactory _playerFactory;

        #endregion

        #region Init

        public PlayerSpawner(IPlayerFactory playerFactory)
        {
            this._playerFactory = playerFactory;
        }

        #endregion

        #region Spawn

        public IPlayer Spawn() =>
            _playerFactory.Create();

        #endregion
    }
}
