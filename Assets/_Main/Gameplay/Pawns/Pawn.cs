using Main.Common.Behaviours;
using Main.Gameplay.Connections;
using UnityEngine;
using Zenject;

namespace Main.Gameplay.Pawns
{
    public class Pawn : AbstractMonoBehaviourExtended, IPoolable<IMemoryPool>, IResetValues
    {
        #region Fields

        private IMemoryPool _pool;
        private PawnDraggable _pawnDraggable;

        #endregion

        #region Inject

        [Inject]
        private void Construct(PawnDraggable pawnDraggable)
        {
            this._pawnDraggable = pawnDraggable;
        }

        #endregion

        #region Spawn

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            Subscribe(true);
        }

        public void OnDespawned()
        {
            _pool = null;
            Subscribe(false);
            ResetValues();
        }

        #endregion

        public void ResetValues()
        { }

        #region Subscribe

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

        #endregion
    }
}
