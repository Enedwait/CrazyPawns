using Main.Common.Behaviours;
using UnityEngine;
using Zenject;

namespace Main.Gameplay.Players
{
    public sealed class PlayerInstaller : MonoInstaller
    {
        #region Fields

        [SerializeField] private PlayerInputHandler _inputHandler;

        #endregion

        #region Install

        public override void InstallBindings()
        {
            Container.Bind<ICameraProvider>()
                .FromInstance(new CameraProvider(null))
                .AsSingle();

            Container.BindInstance(_inputHandler)
                .AsSingle();
        }

        #endregion
    }
}
