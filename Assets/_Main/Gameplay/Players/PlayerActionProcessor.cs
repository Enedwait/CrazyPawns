using Cysharp.Threading.Tasks;
using Main.Common.Behaviours;
using Main.Gameplay.Connectors;
using Main.Gameplay.Managers;
using Main.Gameplay.Pawns;
using System;
using Main.Common.Classes.Objects;
using Main.Gameplay.Targets;

namespace Main.Gameplay.Players
{
    public sealed class PlayerActionProcessor  : AbstractSubscriber
    {
        #region Fields

        private SelectionManager _selectionManager;
        private PanAndZoomManager _panAndZoomManager;
        private DragManager _dragManager;
        private ConnectionManager _connectionManager;

        #endregion

        #region Init

        public PlayerActionProcessor(PlayerActionProcessorParameters parameters)
        {
            this._selectionManager = parameters.SelectionManager;
            this._panAndZoomManager = parameters.PanAndZoomManager;
            this._dragManager = parameters.DragManager;
            this._connectionManager = parameters.ConnectionManager;
        }

        public async UniTask InitializeAsync(PlayerActionProcessorInitArgs args)
        {
            _selectionManager.SetActive(true);

            _panAndZoomManager.SetActive(true);
            _panAndZoomManager.SetPanAllowed(true);
            _panAndZoomManager.SetZoomAllowed(true);
            _panAndZoomManager.SetTarget(args.PanAndZoomTarget);

            _dragManager.SetActive(false);

            _connectionManager.SetActive(false);

            Subscribe(true);
        }

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            SubscribeToSelectionManager(subscribe);
            SubscribeToDragManager(subscribe);
            SubscribeToConnectionManager(subscribe);
        }

        #endregion

        #region SelectionManager

        private void SubscribeToSelectionManager(bool subscribe)
        {
            if (_selectionManager == null)
                return;

            if (subscribe)
            {
                _selectionManager.onSelected += OnSelected;
                _selectionManager.onReleased += OnReleased;
            }
            else
            {
                _selectionManager.onSelected -= OnSelected;
                _selectionManager.onReleased -= OnReleased;
            }
        }

        private void OnSelected(SelectedEventArgs args)
        {
            var selected = args.Selectable;
            ProcessSelectable(selected);
        }

        private void ProcessSelectable(ISelectable selectable)
        {
            switch (selectable)
            {
                case IConnectorSelectable connector: ProcessConnector(connector); break;
                case IPawnSelectable pawn: ProcessPawn(pawn); break;
                default: throw new NotImplementedException($"Unknown type passed: {selectable.GetType().FullName}");
            }
        }

        private void ProcessConnector(IConnectorSelectable selectable)
        {
            _connectionManager.SetActive(true);
            _connectionManager.BeginConnect(selectable.Socket);
        }

        private void ProcessPawn(IPawnSelectable selectable)
        {
            IDraggable draggable = selectable.Draggable;
            if (draggable != null)
            {
                _dragManager.SetActive(true);
                _dragManager.BeginDrag(draggable);
            }
        }

        private void OnReleased(SelectedEventArgs args)
        { }

        #endregion

        #region DragManager

        private void SubscribeToDragManager(bool subscribe)
        {
            if (_dragManager == null)
                return;

            if (subscribe)
            {
                _dragManager.onDragStarted += OnDragStarted;
                _dragManager.onDragCompleted += OnDragCompleted;
            }
            else
            {
                _dragManager.onDragStarted -= OnDragStarted;
                _dragManager.onDragCompleted -= OnDragCompleted;
            }
        }

        private void OnDragStarted(DragStartedEventArgs args)
        {
            _selectionManager.SetActive(false);
            _panAndZoomManager.SetPanAllowed(false);
        }

        private void OnDragCompleted(DragEndedEventArgs args)
        {
            _dragManager.EndDrag();
            _dragManager.SetActive(false);

            _selectionManager.SetActive(true);
            _panAndZoomManager.SetPanAllowed(true);
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

        private void OnConnectionStarted(ConnectionEventArgs args)
        {
            _panAndZoomManager.SetPanAllowed(false);
            _selectionManager.SetActive(false);
        }

        private void OnConnectionEnded(ConnectionEventArgs args)
        {
            _connectionManager.SetActive(false);
            _panAndZoomManager.SetPanAllowed(true);
            _selectionManager.SetActive(true);
        }

        private void OnConnectionEstablished(ConnectionEstablishedEventArgs args)
        {
            _connectionManager.SetActive(false);
            _panAndZoomManager.SetPanAllowed(true);
            _selectionManager.SetActive(true);
        }

        #endregion
    }

    public record PlayerActionProcessorParameters(
        SelectionManager SelectionManager, 
        PanAndZoomManager PanAndZoomManager, 
        DragManager DragManager, 
        ConnectionManager ConnectionManager);

    public record PlayerActionProcessorInitArgs(PanAndZoomTarget PanAndZoomTarget);
}
