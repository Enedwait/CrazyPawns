using Main.Gameplay.Cameras;
using UnityEngine;
using Zenject;

namespace Main.Gameplay.Players
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private PlayerInputHandler _inputHandler;

        public override void InstallBindings()
        {
            Camera camera = Container.Resolve<Camera>();

            Container.Bind<ICameraProvider>()
                .FromInstance(new CameraProviderProvider(camera))
                .AsSingle();

            Container.BindInstance(_inputHandler)
                .AsSingle();
        }
    }
}
