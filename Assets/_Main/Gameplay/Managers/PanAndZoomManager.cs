using Main.Gameplay.Cameras;
using Main.Gameplay.Players;
using Main.Infrastructure.Controls.Providers;
using UnityEngine;
using Zenject;

namespace Main.Gameplay.Managers
{
    [DisallowMultipleComponent]
    public sealed class PanAndZoomManager : AbstractManager
    {
        private CursorPositionProvider _cursorPositionProvider;
        private FloatDeltaProvider _zoomProvider;
        private Vector2DeltaProvider _panProvider;
        private ICameraProvider _cameraProvider;
        private PanAndZoomTarget _target;

        private Camera Camera => _cameraProvider.GetCamera();
        public bool IsPanAllowed { get; private set; }
        public bool IsZoomAllowed { get; private set; }

        [Inject]
        private void Construct(
            ICameraProvider cameraProvider,
            PlayerInputHandler inputHandler)
        {
            this._cameraProvider = cameraProvider;
            this._cursorPositionProvider = inputHandler.CursorPositionProvider;
            this._panProvider = inputHandler.PanProvider;
            this._zoomProvider = inputHandler.ZoomProvider;
        }

        public void SetTarget(PanAndZoomTarget target)
        {
            this._target = target;
        }

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            SubscribeToZoom(subscribe);
            SubscribeToPan(subscribe);
        }

        #endregion

        #region SubscribeToZoom

        private void SubscribeToZoom(bool subscribe)
        {
            if (_zoomProvider == null)
                return;

            if (subscribe)
            {
                _zoomProvider.onDelta += OnZoomDelta;
            }
            else
            {
                _zoomProvider.onDelta -= OnZoomDelta;
            }
        }

        private void OnZoomDelta(float zoom)
        {
            if (!IsActive) return;
            if (_target == null) return;

            Vector2 cursorScreenPosition = _cursorPositionProvider.CursorPosition;
            Ray cameraRay = Camera.ScreenPointToRay(cursorScreenPosition);

            _target.SetZoom(cameraRay.direction, zoom);
        }

        public void SetPanAllowed(bool allowed)
        {
            IsPanAllowed = allowed;
            if (!IsPanAllowed)
                _target?.SetPan(Vector2.zero);
        }

        #endregion

        #region SubscribeToPan

        private void SubscribeToPan(bool subscribe)
        {
            if (_panProvider == null)
                return;

            if (subscribe)
            {
                _panProvider.onDelta += OnPanDelta;
            }
            else
            {
                _panProvider.onDelta -= OnPanDelta;
            }
        }

        private void OnPanDelta(Vector2 panDelta)
        {
            if (!IsActive) return;
            if (!IsPanAllowed) return;
            if (_target == null) return;

            _target.SetPan(panDelta);
        }

        public void SetZoomAllowed(bool allowed)
        {
            IsZoomAllowed = allowed;
            if (!IsZoomAllowed)
                _target?.SetZoom(Vector3.zero, 0f);
        }

        #endregion
    }
}
