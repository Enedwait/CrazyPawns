using Main.Gameplay.Data;
using Main.Gameplay.Pawns;
using Main.Gameplay.Players;
using Main.Gameplay.Connections;
using Main.Gameplay.Managers;
using UnityEngine;
using Zenject;

namespace Main.Gameplay.Installers
{
    public sealed class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private SceneData _sceneData;

        public override void InstallBindings()
        {
            InstallInstances();
            InstallPlayer();
            InstallPawns();
            InstallConnections();
        }

        private void InstallInstances()
        {
            Container.BindInstance(_sceneData)
                .AsSingle();

            Container.BindInstance(_sceneData.MainCamera)
                .AsSingle();
        }

        private void InstallPlayer()
        {
            Container.BindFactory<Player, PlayerFactory>()
                .FromComponentInNewPrefab(_sceneData.Prefabs.Player);

            Container.BindInterfacesAndSelfTo<PlayerSpawner>()
                .AsSingle()
                .NonLazy();
        }

        private void InstallPawns()
        {
            PawnSpawnerParameters parameters = _sceneData.GetPawnSpawnerParameters();

            Container.Bind<PawnPoolSettings>()
                .FromInstance(new PawnPoolSettings(parameters.pawnCount))
                .AsSingle();

            Container.BindMemoryPool<Pawn, PawnPool>()
                .WithInitialSize(parameters.pawnCount)
                .FromComponentInNewPrefab(_sceneData.Prefabs.Pawn)
                .UnderTransformGroup("PawnPool");

            Container.BindInterfacesAndSelfTo<PawnSpawner>()
                .AsSingle()
                .WithArguments(parameters)
                .NonLazy();
        }

        private void InstallConnections()
        {
            Container.Bind<ConnectionPoolSettings>()
                .FromInstance(new ConnectionPoolSettings(_sceneData.InitialConnectionCount))
                .AsSingle();

            Container.BindMemoryPool<Connection, ConnectionPool>()
                .WithInitialSize(_sceneData.InitialConnectionCount)
                .FromComponentInNewPrefab(_sceneData.Prefabs.Connection)
                .UnderTransformGroup("ConnectionPool");

            Container.BindInterfacesAndSelfTo<ConnectionSpawner>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<ConnectionUpdateManager>()
                .AsSingle()
                .NonLazy();
        }
    }
}