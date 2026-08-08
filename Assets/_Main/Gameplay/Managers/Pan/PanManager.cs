using Main.Common.Behaviours;
using Main.Common.Extensions;
using Main.Gameplay.Players;
using Main.Infrastructure.Controls.Providers;
using UnityEngine;
using Zenject;

namespace Main.Gameplay.Managers.Pan
{
    [DisallowMultipleComponent]
    public sealed class PanManager : AbstractManager, IPanManager
    {
        #region Fields

        private IPanTarget _target;
        private IVector2DeltaProvider _panProvider;
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
            this._panProvider = inputHolder.PanProvider;
        }

        #endregion

        #region Methods

        public void SetTarget(IPanTarget target)
        {
            this._target = target;
        }

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            SubscribeToPan(subscribe);
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
            if (_target.IsNullOrDestroyed()) return;

            _target.SetPan(panDelta);
        }

        #endregion
    }
}
