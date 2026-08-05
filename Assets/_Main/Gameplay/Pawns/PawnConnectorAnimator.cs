using Main.Common.Behaviours;
using Main.Gameplay.Animations;
using UnityEngine;

namespace Main.Gameplay.Connectors
{
    public class PawnConnectorAnimator : AbstractPawnEntityAnimator
    {
        [SerializeField] private Connector _connector;

        protected override void InitComponents()
        {
            base.InitComponents();

            if (_connector == null)
                _connector = GetComponent<Connector>();
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
            if (_connector == null)
                return;

            if (subscribe)
            {
                _connector.onSelectedChanged += OnSelectedChanged;
            }
            else
            {
                _connector.onSelectedChanged -= OnSelectedChanged;
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
