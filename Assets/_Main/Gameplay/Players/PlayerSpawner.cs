namespace Main.Gameplay.Players
{
    public sealed class PlayerSpawner : IPlayerSpawner
    {
        #region Fields

        private PlayerFactory _playerFactory;
        private Player _player;

        #endregion

        #region Init

        public PlayerSpawner(PlayerFactory playerFactory)
        {
            this._playerFactory = playerFactory;
        }

        #endregion

        #region Spawn

        public Player Spawn() =>
            _playerFactory.Create();

        #endregion
    }
}
