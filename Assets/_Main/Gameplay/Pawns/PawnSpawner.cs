using System;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Main.Gameplay.Pawns
{
    public sealed class PawnSpawner : IPawnSpawner
    {
        #region Fields

        private PawnPool _pool;
        private Random _random;
        private PawnSpawnerParameters _parameters;

        #endregion

        #region Init

        public PawnSpawner(PawnPool pool, PawnSpawnerParameters parameters)
        {
            this._pool = pool;
            this._parameters = parameters;

            if (_parameters.seed == 0) _parameters.seed = 1;
            if (_parameters.pawnCount < 0) _parameters.pawnCount = 0;
        }

        #endregion

        #region Spawn

        public void SpawnAll()
        {
            _random = new Random(_parameters.seed);
            if (_parameters.doSpawnPawns)
            {
                for (int i = 0; i < _parameters.pawnCount; i++)
                    SpawnOne(_parameters);
            }
        }

        public Pawn SpawnOne(PawnSpawnerParameters parameters)
        {
            PawnSpawnParameters pawnSpawnParameters = new PawnSpawnParameters
            {
                position = GetRandomPosition(parameters.spawnRadius),
            };

            Pawn pawn = _pool.Spawn(pawnSpawnParameters);
            pawn.OnSpawned(_pool);
            return pawn;
        }

        private Vector3 GetRandomPosition(float radius)
        {
            float distance = math.sqrt(_random.NextFloat());
            float2 direction = _random.NextFloat2Direction() * distance * radius;
            return new Vector3(direction.x, 0, direction.y);
        }

        #endregion
    }

    public interface IPawnSpawner
    {
        void SpawnAll();
        Pawn SpawnOne(PawnSpawnerParameters parameters);
    }

    [Serializable]
    public struct PawnSpawnerParameters
    {
        public int pawnCount;
        public float spawnRadius;
        public uint seed;
        public bool doSpawnPawns;
    }

    [Serializable]
    public struct PawnSpawnParameters
    {
        public Vector3 position;
    }
}
