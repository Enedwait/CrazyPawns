using Main.Common.Behaviours;
using Main.Common.Extensions;
using Main.Gameplay.Players;
using Main.Infrastructure.Controls.Providers;
using UnityEngine;
using Zenject;

namespace Main.Gameplay.Managers.Zoom
{
    [DisallowMultipleComponent]
    public sealed class ZoomManager : AbstractManager, IZoomManager
    {
        #region Fields

        private IZoomTarget _target;
        private ICursorPositionProvider _cursorPositionProvider;
        private IFloatDeltaProvider _zoomProvider;
        private ICameraProvider _cameraProvider;

        #endregion

        #region Properties

        private Camera Camera => _cameraProvider.GetCamera();

        #endregion

        #region Inject

        [Inject]
        private void Construct(
            ICameraProvider cameraProvider,
            IPlayerInputHolder inputHolder)
        {
            this._cameraProvider = cameraProvider;
            this._cursorPositionProvider = inputHolder.CursorPositionProvider;
            this._zoomProvider = inputHolder.ZoomProvider;
        }

        #endregion

        #region Methods

        public void SetTarget(IZoomTarget target)
        {
            this._target = target;
        }

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            SubscribeToZoom(subscribe);
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
            if (_target.IsNullOrDestroyed()) return;

            Vector2 cursorScreenPosition = _cursorPositionProvider.CursorPosition;
            Ray cameraRay = Camera.ScreenPointToRay(cursorScreenPosition);

            _target.SetZoom(cameraRay.direction, zoom);
        }

        #endregion
    }
}
