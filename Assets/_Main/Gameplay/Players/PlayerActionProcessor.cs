using Cysharp.Threading.Tasks;
using Main.Common.Behaviours;
using Main.Gameplay.Connectors;
using Main.Gameplay.Pawns;
using System;
using Main.Common.Classes.Objects;
using Main.Gameplay.Managers.Connection;
using Main.Gameplay.Managers.Drag;
using Main.Gameplay.Managers.PanAndZoom;
using Main.Gameplay.Managers.Selection;
using Main.Gameplay.Targets;

namespace Main.Gameplay.Players
{
    public sealed class PlayerActionProcessor  : AbstractSubscriber
    {
        #region Fields

        private ISelectionManager _selectionManager;
        private IPanAndZoomManager _panAndZoomManager;
        private IDragManager _dragManager;
        private IConnectionManager _connectionManager;

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
            // предполагается, что IRL будет асинхронно - ради примера

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
                _selectionManager.onDeselected += OnDeselected;
            }
            else
            {
                _selectionManager.onSelected -= OnSelected;
                _selectionManager.onDeselected -= OnDeselected;
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

        private void OnDeselected(DeselectedEventArgs args)
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

        private void OnConnectionStarted(ConnectionStartedEventArgs args)
        {
            _panAndZoomManager.SetPanAllowed(false);
            _selectionManager.SetActive(false);
        }

        private void OnConnectionEnded(ConnectionEndedEventArgs args)
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
        ISelectionManager SelectionManager, 
        IPanAndZoomManager PanAndZoomManager, 
        IDragManager DragManager, 
        IConnectionManager ConnectionManager);

    public record PlayerActionProcessorInitArgs(PanAndZoomTarget PanAndZoomTarget);
}
