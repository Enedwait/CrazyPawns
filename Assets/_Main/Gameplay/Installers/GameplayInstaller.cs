using Main.Gameplay.Data;
using Main.Gameplay.Pawns;
using Main.Gameplay.Players;
using System.Security.Cryptography;
using Main.Gameplay.Connections;
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

            Container.BindMemoryPool<Pawn, PawnPool>()
                .WithInitialSize(parameters.pawnCount)
                .FromComponentInNewPrefab(_sceneData.Prefabs.Pawn)
                //.UnderTransform(_sceneData.PawnPoolRoot);
                .UnderTransformGroup("PawnPool");

            Container.BindInterfacesAndSelfTo<PawnSpawner>()
                .AsSingle()
                .WithArguments(parameters)
                .NonLazy();
        }

        private void InstallConnections()
        {
            Container.BindMemoryPool<Connection, ConnectionPool>()
                .WithInitialSize(_sceneData.InitialConnectionCount)
                .FromComponentInNewPrefab(_sceneData.Prefabs.Connection)
                .UnderTransformGroup("ConnectionPool");

            Container.BindInterfacesAndSelfTo<ConnectionSpawner>()
                .AsSingle()
                .NonLazy();
        }
    }
}