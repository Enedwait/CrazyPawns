using UnityEngine;
using Zenject;
using Random = Unity.Mathematics.Random;

namespace Main.Gameplay.Pawns
{
    public sealed class PawnSpawner : IInitializable
    {
        private PawnPool _pool;
        private Random _random;
        private PawnSpawnerParameters _parameters;

        public PawnSpawner(PawnPool pool, PawnSpawnerParameters parameters)
        {
            this._pool = pool;
            this._parameters = parameters;
        }

        public void Initialize()
        {
            if (_parameters.seed == 0) 
                _parameters.seed = 1;
            
            _random = new Random(_parameters.seed);

            if (_parameters.doSpawnPawns)
                SpawnAll(_parameters);
        }

        private void SpawnAll(PawnSpawnerParameters parameters)
        {
            for(int i = 0; i < parameters.pawnCount; i++)
                SpawnOne(parameters);
        }

        private void SpawnOne(PawnSpawnerParameters parameters)
        {
            PawnSpawnParameters pawnSpawnParameters = new PawnSpawnParameters
            {
                Position = GetRandomPosition(parameters.spawnRadius),
            };

            Pawn pawn = _pool.Spawn(pawnSpawnParameters);
            pawn.OnSpawned(_pool);
        }

        private Vector3 GetRandomPosition(float radius) =>
            new Vector3(_random.NextFloat(-radius, radius), 0, _random.NextFloat(-radius, radius));
    }
}
