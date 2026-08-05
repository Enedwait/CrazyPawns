using Main.Common.Behaviours;
using Main.Gameplay.Cameras;
using Main.Infrastructure.Controls.Providers;
using UnityEngine;
using Zenject;

namespace Main.Gameplay.Managers
{
    [DisallowMultipleComponent]
    public sealed class PanAndZoomManager : AbstractMonoBehaviourExtended
    {
        [SerializeField] private CursorPositionProvider _cursorPositionProvider;
        [SerializeField] private FloatDeltaProvider _zoomProvider;
        [SerializeField] private Vector2DeltaProvider _panProvider;

        private ICameraProvider _cameraProvider;
        private PanAndZoomTarget _target;

        private Camera Camera => _cameraProvider.GetCamera();

        public bool IsActive { get; private set; }
        public bool IsPanAllowed { get; private set; }
        public bool IsZoomAllowed { get; private set; }

        [Inject]
        private void Construct(ICameraProvider cameraProvider)
        {
            this._cameraProvider = cameraProvider;
        }

        public void SetTarget(PanAndZoomTarget target)
        {
            this._target = target;
        }

        public void SetActive(bool active)
        {
            IsActive = active;
        }

        public void SetPanAllowed(bool allowed)
        {
            IsPanAllowed = allowed;
            if (!IsPanAllowed)
                _target?.SetPan(Vector2.zero);
        }

        public void SetZoomAllowed(bool allowed)
        {
            IsZoomAllowed = allowed;
            if (!IsZoomAllowed)
                _target?.SetZoom(Vector3.zero, 0f);
        }

        private void OnPanDelta(Vector2 panDelta)
        {
            if (!IsActive) return;
            if (!IsPanAllowed) return;
            if (_target == null) return;

            _target.SetPan(panDelta);
        }

        private void OnZoomDelta(float zoom)
        {
            if (!IsActive) return;
            if (_target == null) return;

            Vector2 cursorScreenPosition = _cursorPositionProvider.CursorPosition;
            Ray cameraRay = Camera.ScreenPointToRay(cursorScreenPosition);

            _target.SetZoom(cameraRay.direction, zoom);
        }

        protected override void SubscribeInner(bool subscribe)
        {
            if (subscribe)
            {
                if (_zoomProvider)
                {
                    _zoomProvider.onDelta += OnZoomDelta;
                }

                if (_panProvider)
                {
                    _panProvider.onDelta += OnPanDelta;
                }

                if (_cursorPositionProvider)
                {

                }
            }
            else
            {
                if (_zoomProvider)
                {
                    _zoomProvider.onDelta -= OnZoomDelta;
                }

                if (_panProvider)
                {
                    _panProvider.onDelta -= OnPanDelta;
                }

                if (_cursorPositionProvider)
                {

                }
            }
        }

        protected override void OnDestroy()
        {
            SetActive(false);
            base.OnDestroy();
        }
    }
}
