using System;
using Cysharp.Threading.Tasks;
using Main.Common.Classes.StateMachines;
using Main.Gameplay.Connectors;
using Main.Gameplay.Managers.Connection;
using Main.Gameplay.Managers.Selection;
using Main.Common.Extensions;

namespace Main.Gameplay.Players.States
{
    public class ConnectionState : AbstractPlayerState<IConnectionStateEnterArgs>
    {
        #region Fields

        private ISelectionManager _selectionManager => managerHolder.SelectionManager;
        private IConnectionManager _connectionManager => managerHolder.ConnectionManager;

        #endregion

        #region Init

        public ConnectionState(IManagerHolder managerHolder, IPlayerStateController controller) 
            : base(managerHolder, controller)
        { }

        #endregion

        #region Enter

        public override async UniTask Enter(IConnectionStateEnterArgs args)
        {
            if (args is ConnectionStateEnterArgs connectionArgs)
            {
                IConnectorSelectable selectable = connectionArgs.Selectable;
                if (selectable.IsNullOrDestroyed())
                {
                    controller.ToIdle();
                    return;
                }

                _selectionManager.SetActive(false);

                _connectionManager.SetActive(true);
                _connectionManager.BeginConnect(selectable.Socket);

                Subscribe(true);
            }
            else 
                throw new NotSupportedException($"The state enter arguments of type '{args?.GetType().Name}' are not supported in '{this.GetType().Name}'!");
        }

        #endregion

        #region Exit

        public override async UniTask Exit()
        {
            _connectionManager.SetActive(false);
            _connectionManager.EndConnect();

            await base.Exit();
        }

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            SubscribeToConnectionManager(subscribe);
        }

        #endregion

        #region ConnectionManager

        private void SubscribeToConnectionManager(bool subscribe)
        {
            if (_connectionManager == null)
                return;

            if (subscribe)
            {
                _connectionManager.onConnectionStarted += OnConnectionStarted;
                _connectionManager.onConnectionEnded += OnConnectionEnded;
                _connectionManager.onConnectionEstablished += OnConnectionEstablished;
            }
            else
            {
                _connectionManager.onConnectionStarted -= OnConnectionStarted;
                _connectionManager.onConnectionEnded -= OnConnectionEnded;
                _connectionManager.onConnectionEstablished -= OnConnectionEstablished;
            }
        }

        private void OnConnectionStarted(ConnectionStartedEventArgs args)
        { }

        private void OnConnectionEnded(ConnectionEndedEventArgs args)
        {
            controller.ToIdle();
        }

        private void OnConnectionEstablished(ConnectionEstablishedEventArgs args)
        {
            controller.ToIdle();
        }

        #endregion
    }

    public record ConnectionStateEnterArgs(IConnectorSelectable Selectable) : IConnectionStateEnterArgs;

    public interface IConnectionStateEnterArgs : IStateEnterArgs
    { }
}
