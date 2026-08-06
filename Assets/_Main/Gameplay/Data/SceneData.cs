using System;
using CrazyPawn;
using Main.Gameplay.Cameras;
using Main.Gameplay.Checkerboards;
using Main.Gameplay.Pawns;
using UnityEngine;

namespace Main.Gameplay.Data
{
    public class SceneData : MonoBehaviour
    {
        [field: SerializeField, Header("Scene Objects")] public Camera MainCamera { get; protected set; }
        [field: SerializeField] public PanAndZoomTarget MainPanAndZoomTarget { get; protected set; }
        [field: SerializeField] public Checkerboard Checkerboard { get; protected set; }
        [field: SerializeField, Header("Data")] public PrefabHolderSO Prefabs { get; protected set; }
        [field: SerializeField] public CrazyPawnSettingsExtendedSO Settings { get; protected set; }
        [field: SerializeField, Header("Debug Parameters")] public bool DoSpawnPawns { get; protected set; }

        public CrazyPawnSettings CrazyPawnSettings => Settings.Original;

        public PawnSpawnerParameters GetPawnSpawnerParameters() => new PawnSpawnerParameters
        {
            pawnCount = CrazyPawnSettings.InitialPawnCount,
            spawnRadius = CrazyPawnSettings.InitialZoneRadius,
            seed = (uint)DateTime.UtcNow.Ticks,
            doSpawnPawns = DoSpawnPawns,
        };

        public CheckerboardInitParameters GetCheckerboardInitParameters() => new CheckerboardInitParameters
        {
            boardSize = CrazyPawnSettings.CheckerboardSize,
            cellSize = Settings.CellSize,
            WhiteCellColor = CrazyPawnSettings.WhiteCellColor,
            BlackCellColor = CrazyPawnSettings.BlackCellColor,
        };
    }
}
