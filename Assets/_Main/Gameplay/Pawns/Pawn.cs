using Main.Common.Behaviours;
using Main.Common.Interfaces;
using Main.Gameplay.Connectors;
using Zenject;

namespace Main.Gameplay.Pawns
{
    public class Pawn : AbstractMonoBehaviourExtended, IPoolable<IMemoryPool>, IResetValues
    {
        #region Fields

        private bool _isPooled = false;
        private bool _isDespawned = false;
        private IMemoryPool _pool;
        private IPawnDraggable _pawnDraggable;
        private IConnectorRegistry _connectorRegistry;

        #endregion

        #region Inject

        [Inject]
        private void Construct(
            IPawnDraggable pawnDraggable, 
            IConnectorRegistry connectorRegistry)
        {
            this._pawnDraggable = pawnDraggable;
            this._connectorRegistry = connectorRegistry;
        }

        #endregion

        #region Spawn

        public void OnSpawned(IMemoryPool pool)
        {
            _isPooled = true;
            _isDespawned = false;
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

        #region ResetValues

        public void ResetValues()
        {
            foreach (var connector in _connectorRegistry.Items)
            {
                if (connector == null) continue;
                if (connector.Socket == null) continue;
                if (!this.transform.Equals(connector.Root)) continue;
                connector.Socket.DisconnectAll();
            }
        }

        #endregion

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
            if (_isPooled)
            {
                if (!_isDespawned)
                {
                    _isDespawned = true;
                    _pool.Despawn(this);
                }
            }
            else Destroy(gameObject);
        }

        #endregion
    }
}
