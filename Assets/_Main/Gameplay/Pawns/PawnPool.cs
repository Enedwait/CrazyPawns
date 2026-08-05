using Zenject;

namespace Main.Gameplay.Pawns
{
    public class PawnPool : MonoMemoryPool<PawnSpawnParameters, Pawn>
    {
        protected override void OnCreated(Pawn item)
        {
            base.OnCreated(item);
            item.gameObject.SetActive(false);
        }

        protected override void OnSpawned(Pawn item)
        {
            base.OnSpawned(item);
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(Pawn item)
        {
            base.OnDespawned(item);
            item.gameObject.SetActive(false);
        }

        protected override void Reinitialize(PawnSpawnParameters p1, Pawn item)
        {
            base.Reinitialize(p1, item);
            item.ResetValues();
            item.transform.position = p1.Position;
        }
    }
}
