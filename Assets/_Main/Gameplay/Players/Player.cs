using System;
using System.Data;
using Main.Common.Behaviours;
using Main.Gameplay.Cameras;
using Main.Gameplay.Connectors;
using Main.Gameplay.Data;
using Main.Gameplay.Managers;
using Main.Gameplay.Pawns;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Main.Gameplay.Players
{
    [DisallowMultipleComponent]
    public sealed class Player : AbstractMonoBehaviourExtended
    {
        #region Serialize Field

        [SerializeField] private SelectionManager _selectionManager;
        [SerializeField] private PanAndZoomManager _panAndZoomManager;
        [SerializeField] private DragManager _dragManager;
        [SerializeField] private ConnectionManager _connectionManager;

        #endregion

        #region Fields

        private PlayerInputHandler _inputHandler;
        private SceneData _sceneData;
        private ICameraProvider _cameraProvider;

        #endregion

        #region Properties

        private PlayerInput PlayerInput => _inputHandler.PlayerInput;
        //public PlayerActionState ActionState { get; private set; }

        #endregion

        #region Inject

        [Inject]
        private void Construct(
            SceneData sceneData, 
            ICameraProvider cameraProvider,
            PlayerInputHandler inputHandler)
        {
            this._sceneData = sceneData;
            this._cameraProvider = cameraProvider;
            this._inputHandler = inputHandler;
        }

        #endregion

        #region Unity Methods

        protected override void Start()
        {
            base.Start();

            PlayerInput.camera = _cameraProvider.GetCamera();

            _selectionManager.SetActive(true);

            _panAndZoomManager.SetActive(true);
            _panAndZoomManager.SetPanAllowed(true);
            _panAndZoomManager.SetPanAllowed(true);
            _panAndZoomManager.SetTarget(_sceneData.MainPanAndZoomTarget);

            _dragManager.SetActive(true);

            _connectionManager.SetActive(true);
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
            var selected = args.selectable;
            ProcessSelectable(selected);
        }

        protected void ProcessSelectable(AbstractSelectable selectable)
        {
            switch (selectable)
            {
                case ConnectorSelectable connector: ProcessConnector(connector); break;
                case PawnSelectable pawn: ProcessPawn(pawn); break;
                default: throw new NotImplementedException($"Unknown type passed: {selectable.GetType().FullName}");
            }
        }

        protected void ProcessConnector(ConnectorSelectable selectable)
        {
            _dragManager.EndDrag();

            _panAndZoomManager.SetPanAllowed(false);
            _connectionManager.BeginConnect(selectable.Socket);
        }

        protected void ProcessPawn(PawnSelectable selectable)
        {
            _dragManager.EndDrag();

            PawnDraggable draggable = selectable.PawnDraggable;
            if (draggable != null)
            {
                _dragManager.BeginDrag(draggable);
            }
        }


        private void OnReleased(SelectedEventArgs args)
        {
            var selected = args.selectable;
            _dragManager.EndDrag();
        }

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
            var draggable = args.Draggable; 
            //ActionState = PlayerActionState.Dragging;
            _panAndZoomManager.SetPanAllowed(false);
        }

        private void OnDragCompleted(DragEndedEventArgs args)
        {
            var draggable = args.Draggable;
            //ActionState = PlayerActionState.None;
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
        }

        private void OnConnectionEnded(ConnectionEventArgs args)
        {
            _panAndZoomManager.SetPanAllowed(true);
        }

        private void OnConnectionEstablished(ConnectionEventArgs args)
        {
            _panAndZoomManager.SetPanAllowed(true);
        }

        #endregion
    }

    //public enum PlayerActionState { None, Dragging }
}
