using Main.Gameplay.Data;
using UnityEngine;
using Zenject;

namespace Main.Gameplay.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] protected SceneData sceneData;

        public override void InstallBindings()
        {
            Container.BindInstance(sceneData).AsSingle();
        }
    }
}