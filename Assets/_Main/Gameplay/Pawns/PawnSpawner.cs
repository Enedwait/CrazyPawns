using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Main.Gameplay.Pawns
{
    public sealed class PawnSpawner
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

        public void SpawnAllAsync()
        {
            _random = new Random(_parameters.seed);
            if (_parameters.doSpawnPawns)
            {
                for (int i = 0; i < _parameters.pawnCount; i++)
                    SpawnOne(_parameters);
            }
        }

        private void SpawnOne(PawnSpawnerParameters parameters)
        {
            PawnSpawnParameters pawnSpawnParameters = new PawnSpawnParameters
            {
                position = GetRandomPosition(parameters.spawnRadius),
            };

            Pawn pawn = _pool.Spawn(pawnSpawnParameters);
            pawn.OnSpawned(_pool);
        }

        private Vector3 GetRandomPosition(float radius) =>
            new Vector3(_random.NextFloat(-radius, radius), 0, _random.NextFloat(-radius, radius));

        #endregion
    }
}
