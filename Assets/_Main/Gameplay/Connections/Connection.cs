using Main.Common.Behaviours;
using Main.Gameplay.Connectors;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace Main.Gameplay.Connections
{
    public class Connection : AbstractMonoBehaviourExtended, IPoolable<IMemoryPool>, IResetValues
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private float _width = 0.07f;

        private IMemoryPool _pool;

        public ConnectorSocket SocketA { get; protected set; }
        public ConnectorSocket SocketB { get; protected set; }


#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            _lineRenderer.startWidth = _width;
            _lineRenderer.endWidth = _width;
        }
#endif

        protected override void Awake()
        {
            base.Awake();

            _lineRenderer.useWorldSpace = true;
            _lineRenderer.startWidth = _width;
            _lineRenderer.endWidth = _width;

            ResetValues();
        }

        protected override void InitComponents()
        {
            base.InitComponents();

            if (_lineRenderer == null)
                _lineRenderer = GetComponent<LineRenderer>();
        }

        #region Drag

        public void BeginDrag(Vector3 start)
        {
            _lineRenderer.SetPosition(0, start);
            _lineRenderer.SetPosition(1, start);
        }

        public void Drag(Vector3 position)
        {
            _lineRenderer.SetPosition(1, position);
        }

        public void EndDrag(Vector3 end)
        {
            _lineRenderer.SetPosition(1, end);
        }

        #endregion

        #region Connect

        public virtual bool Connect(ConnectorSocket socketA, ConnectorSocket socketB)
        {
            if (socketA == null || socketB == null)
                return false;

            if (socketA.Root.Equals(socketB.Root))
                return false;

            EndDrag(socketB.transform.position);

            SocketA = socketA;
            SocketB = socketB;

            return true;
        }

        #endregion

        #region Spawn

        public void Remove()
        {
            if (_pool != null)
            {
                _pool.Despawn(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
            Subscribe(true);
        }

        public void OnDespawned()
        {
            _pool = null;
            Subscribe(false);
            ResetValues();
        }

        #endregion

        #region ResetValues

        public void ResetValues()
        {
            _lineRenderer.SetPosition(0, Vector3.zero);
            _lineRenderer.SetPosition(1, Vector3.zero);

            SocketA = null;
            SocketB = null;
        }

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        { }

        #endregion
    }

    public interface IResetValues
    {
        void ResetValues();
    }
}
