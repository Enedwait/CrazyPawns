using Main.Common.Behaviours;
using Main.Gameplay.Pawns.Animations;
using UnityEngine;

namespace Main.Gameplay.Connectors
{
    [DisallowMultipleComponent]
    public class PawnConnectorAnimator : AbstractPawnEntityAnimator
    {
        [SerializeField] private ConnectorSelectable _connectorSelectable;

        protected override void InitComponents()
        {
            base.InitComponents();

            if (_connectorSelectable == null)
                _connectorSelectable = GetComponent<ConnectorSelectable>();
        }

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            base.SubscribeInner(subscribe);
            SubscribeToConnector(subscribe);
        }

        #endregion

        #region Connector

        protected void SubscribeToConnector(bool subscribe)
        {
            if (_connectorSelectable == null)
                return;

            if (subscribe)
            {
                _connectorSelectable.onSelectedChanged += OnSelectedChanged;
            }
            else
            {
                _connectorSelectable.onSelectedChanged -= OnSelectedChanged;
            }
        }

        private void OnSelectedChanged(SelectedChangedEventArgs args)
        {
            if (args.IsSelected)
                ToState(PawnAnimatorState.Active);
            else
                ToState(PawnAnimatorState.Idle);
        }

        #endregion
    }
}
