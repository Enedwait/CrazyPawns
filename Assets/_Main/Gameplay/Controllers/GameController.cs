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

        protected async void Start()
        {
            await _sceneData.Checkerboard.InitializeAsync(_sceneData.GetCheckerboardInitParameters());
        }
    }
}
