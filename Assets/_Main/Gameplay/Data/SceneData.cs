using System;
using CrazyPawn;
using Main.Gameplay.Checkerboards;
using Main.Gameplay.Pawns;
using Main.Gameplay.Players;
using Main.Gameplay.Targets;
using UnityEngine;

namespace Main.Gameplay.Data
{
    public sealed class SceneData : MonoBehaviour
    {
        #region Properties

        [field: SerializeField, Header("Scene Objects")] public Camera MainCamera { get; private set; }
        [field: SerializeField] public PanAndZoomTarget MainPanAndZoomTarget { get; private set; }
        [field: SerializeField] public Checkerboard Checkerboard { get; private set; }
        [field: SerializeField] public GameObject ManualPawns { get; private set; }
        [field: SerializeField, Header("Data")] public PrefabHolderSO Prefabs { get; private set; }
        [field: SerializeField] public CrazyPawnSettingsExtendedSO Settings { get; private set; }
        [field: SerializeField, Header("Debug Parameters")] public bool DoSpawnPawns { get; private set; }

        public CrazyPawnSettings CrazyPawnSettings => Settings.Original;

        #endregion

#if UNITY_EDITOR
        private void OnValidate()
        {
            ManualPawns?.SetActive(!DoSpawnPawns);
        }
#endif

        #region Methods

        public PawnSpawnerParameters GetPawnSpawnerParameters() =>
            new PawnSpawnerParameters
            {
                pawnCount = CrazyPawnSettings.InitialPawnCount,
                spawnRadius = CrazyPawnSettings.InitialZoneRadius,
                seed = (uint)DateTime.UtcNow.Ticks,
                doSpawnPawns = DoSpawnPawns,
            };

        public CheckerboardInitArgs GetCheckerboardInitArgs() => 
            new CheckerboardInitArgs(
            CrazyPawnSettings.CheckerboardSize, 
            Settings.CellSize, 
            CrazyPawnSettings.WhiteCellColor, 
            CrazyPawnSettings.BlackCellColor);

        public PlayerInitArgs GetPlayerInitArgs() =>
            new PlayerInitArgs(MainCamera, MainPanAndZoomTarget);

        #endregion
    }
}
