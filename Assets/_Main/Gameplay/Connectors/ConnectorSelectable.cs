using Main.Common.Behaviours;
using UnityEngine;

namespace Main.Gameplay.Connectors
{
    [DisallowMultipleComponent]
    public class ConnectorSelectable : AbstractSelectable, IConnectorSelectable
    {
        [SerializeField] private ConnectorSocket _socket;

        public IConnectorSocket Socket => _socket;

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

    public interface IConnectorSelectable : ISelectable
    {
        IConnectorSocket Socket { get; }
    }
}
