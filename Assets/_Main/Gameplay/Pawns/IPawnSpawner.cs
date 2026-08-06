namespace Main.Gameplay.Pawns
{
    public interface IPawnSpawner
    {
        void SpawnAll();
        Pawn SpawnOne(PawnSpawnerParameters parameters);
    }
}
