using System;
using Main.Common.Classes.Pools;

namespace Main.Gameplay.Pawns
{
    public class PawnPool : TrackedMonoPool<PawnSpawnParameters, Pawn, PawnPoolSettings>
    {
        protected override void OnCreated(Pawn item)
        {
            base.OnCreated(item);
            if (item == null) return;
            item.gameObject.SetActive(false);
        }

        protected override void OnSpawned(Pawn item)
        {
            base.OnSpawned(item);
            if (item == null) return;
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(Pawn item)
        {
            base.OnDespawned(item);
            if (item == null) return;
            item.gameObject.SetActive(false);
        }

        protected override void Reinitialize(PawnSpawnParameters p1, Pawn item)
        {
            base.Reinitialize(p1, item);
            if (item == null) return;
            item.ResetValues();
            item.transform.position = p1.Position;
        }
    }

    [Serializable]
    public class PawnPoolSettings : AbstractTrackedPoolSettings
    {
        public PawnPoolSettings(int initialCapacity) : base(initialCapacity)
        { }
    }
}
