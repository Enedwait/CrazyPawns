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
        [field: SerializeField] public Transform PawnPoolRoot { get; protected set; }
        [field: SerializeField] public Transform PawnSpawnRoot { get; protected set; }
        [field: SerializeField, Header("Data")] public PrefabHolderSO Prefabs { get; protected set; }
        [field: SerializeField] public CrazyPawnSettings CrazyPawnSettings { get; protected set; }
        [field: SerializeField] public float CellSize { get; protected set; } = 1.5f;

        [field: SerializeField, Header("Parameters")] public bool DoSpawnPawns { get; protected set; }

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
            cellSize = CellSize,
            WhiteCellColor = CrazyPawnSettings.WhiteCellColor,
            BlackCellColor = CrazyPawnSettings.BlackCellColor,
        };
    }
}
