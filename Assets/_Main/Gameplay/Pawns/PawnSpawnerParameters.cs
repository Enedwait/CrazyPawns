using System;

namespace Main.Gameplay.Pawns
{
    [Serializable]
    public struct PawnSpawnerParameters
    {
        public int pawnCount;
        public float spawnRadius;
        public uint seed;
        public bool doSpawnPawns;
    }
}
