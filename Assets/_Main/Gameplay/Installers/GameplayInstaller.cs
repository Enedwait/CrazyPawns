using Main.Gameplay.Data;
using Main.Gameplay.Pawns;
using Main.Gameplay.Players;
using Main.Gameplay.Connections;
using Main.Gameplay.Connectors;
using Main.Gameplay.Managers;
using UnityEngine;
using Zenject;

namespace Main.Gameplay.Installers
{
    public sealed class GameplayInstaller : MonoInstaller
    {
        #region Fields

        [SerializeField] private SceneData _sceneData;

        #endregion

        #region Install

        public override void InstallBindings()
        {
            InstallInstances();
            InstallPlayer();
            InstallPawns();
            InstallConnectors();
            InstallConnections();
        }

        #endregion

        #region Instances

        private void InstallInstances()
        {
            Container.BindInstance(_sceneData)
                .AsSingle();

            Container.BindInstance(_sceneData.MainCamera)
                .AsSingle();
        }

        #endregion

        #region Player

        private void InstallPlayer()
        {
            Container.BindFactory<Player, PlayerFactory>()
                .FromComponentInNewPrefab(_sceneData.Prefabs.Player);

            Container.BindInterfacesAndSelfTo<PlayerSpawner>()
                .AsSingle()
                .NonLazy();
        }

        #endregion

        #region Pawns

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

        #endregion

        #region Connectors

        private void InstallConnectors()
        {
            Container.Bind<ConnectorRegistry>()
                .FromInstance(new ConnectorRegistry(_sceneData.CrazyPawnSettings.InitialPawnCount * 5))
                .AsSingle()
                .NonLazy();
        }

        #endregion

        #region Connections

        private void InstallConnections()
        {
            int initialConnectionCount = _sceneData.Settings.InitialConnectionCount;

            Container.Bind<ConnectionPoolSettings>()
                .FromInstance(new ConnectionPoolSettings(initialConnectionCount))
                .AsSingle();

            Container.BindMemoryPool<Connection, ConnectionPool>()
                .WithInitialSize(initialConnectionCount)
                .FromComponentInNewPrefab(_sceneData.Prefabs.Connection)
                .UnderTransformGroup("ConnectionPool");

            Container.Bind<IActiveConnectionItems>()
                .To<ConnectionPool>()
                .FromResolve();

            Container.BindInterfacesAndSelfTo<ConnectionSpawner>()
                .AsSingle()
                .NonLazy();

            Container.BindInterfacesAndSelfTo<ConnectionUpdateManager>()
                .AsSingle()
                .NonLazy();
        }

        #endregion
    }
}