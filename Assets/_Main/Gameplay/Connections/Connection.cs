using Main.Common.Behaviours;
using Main.Common.Interfaces;
using Main.Gameplay.Connectors;
using UnityEngine;
using Zenject;

namespace Main.Gameplay.Connections
{
    public sealed class Connection : AbstractMonoBehaviourExtended, IPoolable<IMemoryPool>, IResetValues, IConnection
    {
        #region Fields

        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _width = 0.07f;

        private bool _isPooled = false;
        private bool _isDespawned = false;
        private IMemoryPool _pool;
        private IActiveConnectionItems _activeConnections;
        private IConnectionSettingsProvider _connectionSettingsProvider;

        #endregion

        #region Properties

        public bool IsConnected { get; private set; }
        public IConnectorSocket SocketA { get; private set; }
        public IConnectorSocket SocketB { get; private set; }

        #endregion

        #region Inject

        [Inject]
        private void Construct(
            IConnectionSettingsProvider connectionSettingsProvider,
            IActiveConnectionItems activeConnections)
        {
            this._connectionSettingsProvider = connectionSettingsProvider;
            this._activeConnections = activeConnections;
        }

        #endregion

        #region Unity Methods

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            _lineRenderer.useWorldSpace = true;
            SetWidth(_width);
        }
#endif

        protected override void Awake()
        {
            base.Awake();
            
            _width = _connectionSettingsProvider.GetSettings().connectionWidth;
            
            _lineRenderer.useWorldSpace = true;
            SetWidth(_width);

            ResetValues();
        }

        #endregion

        #region Init

        protected override void InitComponents()
        {
            base.InitComponents();

            if (_lineRenderer == null)
                _lineRenderer = GetComponent<LineRenderer>();
        }

        #endregion

        #region Drag

        public void MoveStartAt(Vector3 start)
        {
            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, start);
        }

        public void MoveEndAt(Vector3 end)
        {
            _lineRenderer.SetPosition(1, end);
        }

        #endregion

        #region Connect

        public bool TryConnect(IConnectorSocket socketA, IConnectorSocket socketB, out ConnectionFailedReason failed)
        {
            IsConnected = false;
            failed = ConnectionFailedReason.None;

            if (socketA == null || socketB == null)
            {
                failed = ConnectionFailedReason.NoSocket;
                return false;
            }

            if (socketA.Root.Equals(socketB.Root))
            {
                failed = ConnectionFailedReason.SameRoot;
                return false;
            }

            SocketA = socketA;
            SocketB = socketB;

            foreach (Connection other in _activeConnections.ActiveItems)
            {
                if (other == null) continue;
                if (this.Equals(other)) continue;
                if (!IsSame(other)) continue;

                failed = ConnectionFailedReason.AlreadyExists;
                return false;
            }

            SocketA.Connect(this);
            SocketB.Connect(this);
            
            MoveEndAt(socketB.Position);
            IsConnected = true;
            return true;
        }

        private void UpdateA()
        {
            if (!IsConnected || SocketA == null) return;
            _lineRenderer.SetPosition(0, SocketA.Position);
        }

        private void UpdateB()
        {
            if (!IsConnected || SocketB == null) return;
            _lineRenderer.SetPosition(1, SocketB.Position);
        }

        public void UpdatePoints()
        {
            UpdateA();
            UpdateB();
        }

        #endregion

        #region Disconnect

        public void Disconnect()
        {
            IsConnected = false;
            if (SocketA != null) SocketA.Disconnect(this);
            if (SocketB != null) SocketB.Disconnect(this);
            Remove();
        }

        #endregion

        #region Spawn

        private void Remove()
        {
            if (_isPooled)
            {
                if (!_isDespawned)
                {
                    _isDespawned = true;
                    _pool.Despawn(this);
                }
            }
            else Destroy(gameObject);
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _isPooled = true;
            _isDespawned = false;
            _pool = pool;
            Subscribe(true);
        }

        public void OnDespawned()
        {
            _isDespawned = true;
            _pool = null;
            Subscribe(false);
            ResetValues();
        }

        #endregion

        public void SetWidth(float width)
        {
            if (width < 0.01f)
                width = 0.01f;

            _width = width;
            _lineRenderer.startWidth = _width;
            _lineRenderer.endWidth = _width;
        }

        #region ResetValues

        public void ResetValues()
        {
            _lineRenderer.SetPosition(0, Vector3.zero);
            _lineRenderer.SetPosition(1, Vector3.zero);

            SocketA = null;
            SocketB = null;
        }

        #endregion

        #region Comparison

        public bool HasBothSockets() => SocketA != null && SocketB != null;
        public bool HasOneSocket() => (SocketA != null && SocketB == null) || (SocketA == null && SocketB != null);
        public bool HasNoSockets() => SocketA == null && SocketB == null;
        public IConnectorSocket GetAnySocket() => SocketA ?? SocketB;

        public bool IsSame(Connection other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other)) 
                return true;

            if (SocketA == null)
            {
                if (SocketB == null)
                    return other.HasNoSockets();

                return other.HasOneSocket() && SocketB.Equals(other.GetAnySocket());
            }

            if (SocketB == null)
                return other.HasOneSocket() && SocketA.Equals(other.GetAnySocket());

            return other.HasBothSockets() 
                   && ((SocketA.Equals(other.SocketA) && SocketB.Equals(other.SocketB))
                   || (SocketA.Equals(other.SocketB) && SocketB.Equals(other.SocketA)));
        }

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        { }

        #endregion
    }
}
