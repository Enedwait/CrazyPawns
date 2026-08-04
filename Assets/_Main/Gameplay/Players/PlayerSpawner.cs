using Zenject;

namespace Main.Gameplay.Players
{
    public class PlayerSpawner : IInitializable
    {
        private PlayerFactory _playerFactory;
        private Player _player;

        public PlayerSpawner(PlayerFactory playerFactory)
        {
            this._playerFactory = playerFactory;
        }

        public void Initialize()
        {
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            _player = _playerFactory.Create();
        }
    }
}
