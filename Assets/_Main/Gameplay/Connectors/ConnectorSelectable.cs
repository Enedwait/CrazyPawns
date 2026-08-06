using Main.Common.Behaviours;
using UnityEngine;

namespace Main.Gameplay.Connectors
{
    [DisallowMultipleComponent]
    public class ConnectorSelectable : AbstractSelectable, IConnectorSelectable
    {
        #region Fields

        [SerializeField] private ConnectorSocket _socket;

        #endregion

        #region Properties

        public IConnectorSocket Socket => _socket;

        #endregion

        #region Init

        protected override void InitComponents()
        {
            base.InitComponents();

            if (_socket == null)
                _socket = GetComponent<ConnectorSocket>();
        }

        #endregion

        #region Select

        protected override bool SelectInner()
        {
            return true;
        }

        protected override bool DeselectInner()
        {
            return true;
        }

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        { }

        #endregion
    }

    public interface IConnectorSelectable : ISelectable
    {
        IConnectorSocket Socket { get; }
    }
}
