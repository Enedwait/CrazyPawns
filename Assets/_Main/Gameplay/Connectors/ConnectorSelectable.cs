using Main.Common.Behaviours;
using UnityEngine;

namespace Main.Gameplay.Connectors
{
    [DisallowMultipleComponent]
    public class ConnectorSelectable : AbstractSelectable
    {
        [SerializeField] private ConnectorSocket _socket;

        public ConnectorSocket Socket => _socket;

        protected override void InitComponents()
        {
            base.InitComponents();
            if (_socket == null)
                _socket = GetComponent<ConnectorSocket>();
        }

        protected override bool SelectInner()
        {
            return true;
        }

        protected override bool DeselectInner()
        {
            return true;
        }

        protected override void SubscribeInner(bool subscribe)
        { }
    }
}
