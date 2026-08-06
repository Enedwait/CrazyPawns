using Main.Common.Behaviours;
using Main.Common.Extensions;
using Main.Gameplay.Connections;
using Main.Gameplay.Connectors;
using Main.Gameplay.Players;
using Main.Infrastructure.Controls.Providers;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Main.Gameplay.Managers.Connection
{
    public class ConnectionManager : AbstractManager, IConnectionManager
    {
        #region Fields

        [SerializeField] protected int _maxHitsCount = 16;
        [SerializeField] private float _maxDistance = 1000f;
        [SerializeField] private LayerMask _layersToCheck;

        private bool isFirstClick = true;
        private RaycastHit[] _hits;

        private IClickProvider _clickProvider;
        private IVector2DeltaProvider _cursorDeltaProvider;
        private ICursorPositionProvider _cursorPositionProvider;
        private ICameraProvider _cameraProvider;
        private IConnectionSpawner _connectionSpawner;
        private IConnectorRegistry _connectorRegistry;

        #endregion

        #region Properties

        private Camera Camera => _cameraProvider.GetCamera();

        public bool IsConnecting { get; private set; }
        public bool IsMoving { get; private set; }
        public IConnectorSocket SocketA { get; private set; }
        public IConnectorSocket SocketB { get; private set; }
        public Connections.Connection Current { get; private set; }

        #endregion

        #region Events

        public event UnityAction<ConnectionStartedEventArgs> onConnectionStarted;
        public event UnityAction<ConnectionEndedEventArgs> onConnectionEnded;
        public event UnityAction<ConnectionEstablishedEventArgs> onConnectionEstablished;

        #endregion

        #region Inject

        [Inject]
        private void Construct(
            IConnectorRegistry connectorRegistry,
            ICameraProvider cameraProvider, 
            IConnectionSpawner connectionSpawner,
            PlayerInputHandler inputHandler)
        {
            this._connectorRegistry = connectorRegistry;
            this._cameraProvider = cameraProvider;
            this._connectionSpawner = connectionSpawner;
            this._clickProvider = inputHandler.ClickProvider;
            this._cursorPositionProvider = inputHandler.CursorPositionProvider;
            this._cursorDeltaProvider = inputHandler.PanProvider;
        }

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            _hits = new RaycastHit[_maxHitsCount];
        }

        #endregion

        #region Connect

        public void BeginConnect(IConnectorSocket from)
        {
            if (!IsActive || from == null)
                return;

            isFirstClick = true;
            SocketA = from;
            IsConnecting = true;

            Current = _connectionSpawner.Spawn();
            Current.BeginDrag(SocketA.Position);

            ActivateConnectorsExceptFor(SocketA.Root);

            RaiseOnConnectionStarted(new ConnectionStartedEventArgs(this, Current));
        }

        #endregion

        #region EndConnect

        public void EndConnect()
        {
            if (!IsActive || !IsConnecting)
                return;

            DeactivateConnectorsExceptFor(null);
            IsConnecting = false;
            IsMoving = false;

            Vector2 screenPosition = _cursorPositionProvider.CursorPosition;
            Ray cameraRay = Camera.ScreenPointToRay(screenPosition);

            int hitsCount = Physics.RaycastNonAlloc(cameraRay, _hits, _maxDistance, _layersToCheck);
            if (hitsCount > 0)
            {
                RaycastHit closestHit = _hits.GetClosestHit(hitsCount);

                ConnectorSocket socket = closestHit.collider.GetComponent<ConnectorSocket>();
                if (socket != null)
                {
                    SocketB = socket;

                    if (Current.TryConnect(SocketA, SocketB, out ConnectionFailedReason failed))
                    {
                        RaiseOnConnectionEstablished(new ConnectionEstablishedEventArgs(this, Current));
                        Current = null;
                        return;
                    }
                    else
                    {
                        Debug.LogWarning($"Не могу соединить: {failed.ToMessage()}");
                    }
                }
            }

            Current?.Disconnect();
            RaiseOnConnectionEnded(new ConnectionEndedEventArgs(this, Current));
            Current = null;
        }

        #endregion

        #region ActivateConnectors

        private void ActivateConnectorsExceptFor(Transform root) =>
            SetStateOfConnectorsExceptFor(root, PawnConnectorAnimator.ConnectorAnimatorState.ReadyToConnect);

        private void DeactivateConnectorsExceptFor(Transform root) =>
            SetStateOfConnectorsExceptFor(root, PawnConnectorAnimator.ConnectorAnimatorState.Idle);

        private void SetStateOfConnectorsExceptFor(Transform root, PawnConnectorAnimator.ConnectorAnimatorState state)
        {
            if (root == null)
            {
                foreach (var connector in _connectorRegistry.Items)
                {
                    if (connector == null) continue;
                    connector.Animator.ToState(state);
                }

                return;
            }

            foreach (var connector in _connectorRegistry.Items)
            {
                if (connector == null) continue;
                if (connector.Socket == null) continue;
                if (root.Equals(connector.Socket.Root)) continue;

                connector.Animator.ToState(state);
            }
        }

        #endregion

        #region Event Raisers

        private void RaiseOnConnectionStarted(ConnectionStartedEventArgs args) => onConnectionStarted?.Invoke(args);
        private void RaiseOnConnectionEnded(ConnectionEndedEventArgs args) => onConnectionEnded?.Invoke(args);
        private void RaiseOnConnectionEstablished(ConnectionEstablishedEventArgs args) => onConnectionEstablished?.Invoke(args);

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            SubscribeToClick(subscribe);
            SubscribeToCursorDelta(subscribe);
        }

        #endregion

        #region Click

        private void SubscribeToClick(bool subscribe)
        {
            if (_clickProvider == null)
                return;

            if (subscribe)
            {
                _clickProvider.onClickCanceled += OnClick;
            }
            else
            {
                _clickProvider.onClickCanceled -= OnClick;
            }
        }

        private void OnClick()
        {
            if (IsMoving)
            {
                EndConnect();
            }
            else
            {
                if (isFirstClick) isFirstClick = false;
                else EndConnect();
            }
        }

        #endregion

        #region Connection Moving

        private void SubscribeToCursorDelta(bool subscribe)
        {
            if (_cursorDeltaProvider == null)
                return;

            if (subscribe)
            {
                _cursorDeltaProvider.onDelta += OnMoveDelta;
            }
            else
            {
                _cursorDeltaProvider.onDelta -= OnMoveDelta;
            }
        }

        private void OnMoveDelta(Vector2 delta)
        {
            MoveConnection(delta);
        }

        public void MoveConnection(Vector2 delta)
        {
            if (!IsActive || !IsConnecting)
                return;

            if (SocketA.IsNullOrDestroyed()) 
            {
                EndConnect();
                return;
            }

            IsMoving = true;

            Vector3 start = SocketA.Position;
            Vector3 end = start;

            Vector2 screenPosition = _cursorPositionProvider.CursorPosition;
            Ray cameraRay = Camera.ScreenPointToRay(screenPosition);

            int hitsCount = Physics.RaycastNonAlloc(cameraRay, _hits, _maxDistance, _layersToCheck);
            if (hitsCount > 0)
            {
                RaycastHit closestHit = _hits.GetClosestHit(hitsCount);
                end = closestHit.point;
            }
            else
                end = _cursorPositionProvider.GetWorldPositionWithY(Camera, 0f);

            Current.Drag(end);
        }

        #endregion
    }
}
