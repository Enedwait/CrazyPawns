using Main.Common.Behaviours;
using Zenject;

namespace Main.Gameplay.Pawns
{
    public class Pawn : AbstractMonoBehaviourExtended, IPoolable<IMemoryPool>
    {
        private IMemoryPool _pool;
        private PawnDraggable _pawnDraggable;

        [Inject]
        private void Construct(PawnDraggable pawnDraggable)
        {
            this._pawnDraggable = pawnDraggable;
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            Subscribe(true);
        }

        public void OnDespawned()
        {
            _pool = null;
            Subscribe(false);
        }

        protected override void SubscribeInner(bool subscribe)
        {
            if (_pawnDraggable == null) 
                return;

            if (subscribe)
            {
                _pawnDraggable.onDragEndedOutside += OnDragEndedOutside;
            }
            else
            {
                _pawnDraggable.onDragEndedOutside -= OnDragEndedOutside;
            }
        }

        private void OnDragEndedOutside(PawnDragEndedOutsideEventArgs args)
        {
            if (_pool != null)
            {
                _pool.Despawn(this);
            }
            else
            {
                Destroy(transform.gameObject);
            }
        }
    }
}
