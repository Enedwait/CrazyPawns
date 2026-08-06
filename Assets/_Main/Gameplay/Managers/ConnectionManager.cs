using Main.Common.Extensions;
using Main.Gameplay.Cameras;
using Main.Gameplay.Connections;
using Main.Gameplay.Connectors;
using Main.Gameplay.Players;
using Main.Infrastructure.Controls.Providers;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Main.Gameplay.Managers
{
    public class ConnectionManager : AbstractManager
    {
        [SerializeField] protected int _maxHitsCount = 16;
        [SerializeField] private float _maxDistance = 1000f;
        [SerializeField] private LayerMask _layersToCheck;

        private ClickProvider _clickProvider;
        private Vector2DeltaProvider _cursorDeltaProvider;
        private CursorPositionProvider _cursorPositionProvider;

        private bool isFirstClick = true;
        private RaycastHit[] _hits;
        private ICameraProvider _cameraProvider;
        private ConnectionSpawner _connectionSpawner;
        private Camera Camera => _cameraProvider.GetCamera();

        public bool IsConnecting { get; private set; }
        public bool IsMoving { get; private set; }
        public IConnectorSocket SocketA { get; private set; }
        public IConnectorSocket SocketB { get; private set; }
        public Connection Current { get; private set; }

        public event UnityAction<ConnectionEventArgs> onConnectionStarted;
        public event UnityAction<ConnectionEventArgs> onConnectionEnded;
        public event UnityAction<ConnectionEventArgs> onConnectionEstablished;

        [Inject]
        private void Construct(
            ICameraProvider cameraProvider, 
            ConnectionSpawner connectionSpawner,
            PlayerInputHandler inputHandler)
        {
            this._cameraProvider = cameraProvider;
            this._connectionSpawner = connectionSpawner;
            this._clickProvider = inputHandler.ClickProvider;
            this._cursorPositionProvider = inputHandler.CursorPositionProvider;
            this._cursorDeltaProvider = inputHandler.PanProvider;
        }

        protected override void Awake()
        {
            base.Awake();

            _hits = new RaycastHit[_maxHitsCount];
        }

        public void BeginConnect(IConnectorSocket A)
        {
            if (!IsActive || A == null)
                return;

            isFirstClick = true;
            SocketA = A;
            IsConnecting = true;

            Current = _connectionSpawner.Spawn();
            Current.BeginDrag(SocketA.Position);

            RaiseOnConnectionStarted(new ConnectionEventArgs(this, Current));
        }

        public void EndConnect()
        {
            if (!IsActive || !IsConnecting)
                return;

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
                        RaiseOnConnectionEstablished(new ConnectionEventArgs(this, Current));
                        Current = null;
                        return;
                    }
                    else
                    {
                        Debug.LogWarning($"Не могу соединить: {failed.ToMessage()}");
                    }
                }
            }

            FinalizeEndConnect();
        }

        private void FinalizeEndConnect()
        {
            IsConnecting = false;
            IsMoving = false;

            if (Current != null)
            {
                Current.Disconnect();
                RaiseOnConnectionEnded(new ConnectionEventArgs(this, Current));
                Current = null;
            }
        }

        private void RaiseOnConnectionStarted(ConnectionEventArgs args) => onConnectionStarted?.Invoke(args);
        private void RaiseOnConnectionEnded(ConnectionEventArgs args) => onConnectionEnded?.Invoke(args);
        private void RaiseOnConnectionEstablished(ConnectionEventArgs args) => onConnectionEstablished?.Invoke(args);

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

            if (SocketA.IsNullAsComponent()) 
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

    public record ConnectionEventArgs(ConnectionManager Manager, Connection Connection);
    public record ConnectionEstablishedEventArgs(ConnectionManager Manager, Connection Connection);
}
