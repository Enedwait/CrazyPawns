using Main.Gameplay.Data;
using Main.Gameplay.Managers;
using Main.Gameplay.Pawns;
using Main.Gameplay.Players;
using UnityEngine;
using Zenject;

namespace Main.Gameplay.Controllers
{
    public class GameController : MonoBehaviour
    {
        #region Fields

        private SceneData _sceneData;
        private PlayerSpawner _playerSpawner;
        private PawnSpawner _pawnSpawner;
        private ConnectionUpdateManager _connectionUpdateManager;

        #endregion

        #region Inject

        [Inject]
        private void Construct(
            SceneData sceneData, 
            PlayerSpawner playerSpawner,
            PawnSpawner pawnSpawner,
            ConnectionUpdateManager connectionUpdateManager)
        {
            this._sceneData = sceneData;
            this._playerSpawner = playerSpawner;
            this._pawnSpawner = pawnSpawner;
            this._connectionUpdateManager = connectionUpdateManager;
        }

        #endregion

        #region Unity Methods

        protected async void Start()
        {
            Debug.Log($"Инициализация доски...");
            await _sceneData.Checkerboard.InitializeAsync(_sceneData.GetCheckerboardInitArgs());

            Debug.Log($"Инициализация пешек...");
            _pawnSpawner.SpawnAllAsync();

            Debug.Log($"Инициализация игрока...");
            Player player = _playerSpawner.SpawnPlayer();
            await player.InitializeAsync(_sceneData.GetPlayerInitArgs());

            _connectionUpdateManager.SetActive(true);

            Debug.Log($"Игра начата!");
        }

        #endregion
    }
}
