using Main.Gameplay.Checkerboards;
using Main.Gameplay.Data;
using UnityEngine;
using Zenject;

namespace Main.Gameplay.Controllers
{
    public class GameController : MonoBehaviour
    {
        private SceneData _sceneData;

        [Inject]
        private void Construct(SceneData sceneData)
        {
            this._sceneData = sceneData;
        }

        protected void Start()
        {
            _sceneData.Checkerboard.Initialize(new CheckerboardInitParameters
            {
                boardSize = _sceneData.CrazyPawnSettings.CheckerboardSize,
                cellSize = 1.5f,
                WhiteCellColor = _sceneData.CrazyPawnSettings.WhiteCellColor,
                BlackCellColor = _sceneData.CrazyPawnSettings.BlackCellColor,
            });
        }
    }
}
