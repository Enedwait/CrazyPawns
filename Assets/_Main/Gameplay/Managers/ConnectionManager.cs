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

        private RaycastHit[] _hits;
        private ICameraProvider _cameraProvider;
        private ConnectionSpawner _connectionSpawner;
        private Camera Camera => _cameraProvider.GetCamera();
        
        public bool IsConnecting { get; private set; }
        public ConnectorSocket SocketA { get; private set; }
        public ConnectorSocket SocketB { get; private set; }
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
            this._cursorDeltaProvider = inputHandler.CursorDeltaProvider;
        }

        protected override void Awake()
        {
            base.Awake();

            _hits = new RaycastHit[_maxHitsCount];
        }

        public void BeginConnect(ConnectorSocket A)
        {
            if (!IsActive || A == null)
                return;

            SocketA = A;
            IsConnecting = true;

            Current = _connectionSpawner.Spawn();
            Current.BeginDrag(SocketA.transform.position);

            RaiseOnConnectionStarted(new ConnectionEventArgs(this, Current));
        }

        public void EndConnect()
        {
            if (!IsActive || Current == null)
            {
                FinalizeEndConnect();
                return;
            }

            Vector2 screenPosition = _cursorPositionProvider.CursorPosition;
            Ray cameraRay = Camera.ScreenPointToRay(screenPosition);

            int hitsCount = Physics.RaycastNonAlloc(cameraRay, _hits, _maxDistance, _layersToCheck);
            if (hitsCount > 0)
            {
                RaycastHit closestHit = _hits.GetClosestHit(hitsCount);

                ConnectorSocket socket = closestHit.collider.GetComponent<ConnectorSocket>();
                if (socket != null)
                {
                    IsConnecting = false;
                    SocketB = socket;
                    if (Current.Connect(SocketA, SocketB))
                    {
                        RaiseOnConnectionEstablished(new ConnectionEventArgs(this, Current));
                        Current = null;
                        return;
                    }
                }
            }

            FinalizeEndConnect();
        }

        private void FinalizeEndConnect()
        {
            IsConnecting = false;
            if (Current != null)
            {
                Current.Remove();
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

        #region SubscribeToClick

        private void SubscribeToClick(bool subscribe)
        {
            if (_clickProvider == null)
                return;

            if (subscribe)
            {
                _clickProvider.onClickCanceled += OnClickCanceled;
            }
            else
            {
                _clickProvider.onClickCanceled -= OnClickCanceled;
            }
        }

        private void OnClickCanceled()
        {
            EndConnect();
        }

        #endregion

        #region SubscribeToCursorDelta

        private void SubscribeToCursorDelta(bool subscribe)
        {
            if (_cursorDeltaProvider == null)
                return;

            if (subscribe)
            {
                _cursorDeltaProvider.onDelta += OnCursorDelta;
            }
            else
            {
                _cursorDeltaProvider.onDelta -= OnCursorDelta;
            }
        }

        private void OnCursorDelta(Vector2 delta)
        {
            if (!IsActive || !IsConnecting)
                return;

            Vector3 start = SocketA.transform.position;
            Vector3 end = start;

            //Vector3 direction = Vector3.zero;
            Vector2 screenPosition = _cursorPositionProvider.CursorPosition;
            Ray cameraRay = Camera.ScreenPointToRay(screenPosition);

            int hitsCount = Physics.RaycastNonAlloc(cameraRay, _hits, _maxDistance, _layersToCheck);
            if (hitsCount > 0)
            {
                RaycastHit closestHit = _hits.GetClosestHit(hitsCount);
                end = closestHit.point;
                //direction = closestHit.transform.position - start;
            }
            else
            {
                end = _cursorPositionProvider.GetWorldPositionWithY(Camera, 0f);
                //direction = (worldPosition - start);
            }

            //Current.Drag(direction);
            Current.Drag(end);
        }

        #endregion
    }

    public record ConnectionEventArgs(ConnectionManager Manager, Connection Connection);
    public record ConnectionEstablishedEventArgs(ConnectionManager Manager, Connection Connection);
}
