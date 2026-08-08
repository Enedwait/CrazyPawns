using Cysharp.Threading.Tasks;
using Main.Common.Behaviours;
using Main.Common.Classes.StateMachines;
using Main.Gameplay.Connectors;
using Main.Gameplay.Managers.Connection;
using Main.Gameplay.Managers.Drag;
using Main.Gameplay.Managers.Pan;
using Main.Gameplay.Managers.Selection;
using Main.Gameplay.Managers.Zoom;
using Main.Gameplay.Players.States;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Main.Gameplay.Players
{
    [DisallowMultipleComponent]
    public sealed class Player : AbstractMonoBehaviourExtended, IPlayer
    {
        #region Serialize Field

        [SerializeField] private SelectionManager _selectionManager;
        [SerializeField] private PanManager _panManager;
        [SerializeField] private ZoomManager _zoomManager;
        [SerializeField] private DragManager _dragManager;
        [SerializeField] private ConnectionManager _connectionManager;

        #endregion

        #region Fields

        private IManagerHolder _managerHolder;
        private IPlayerStateController _stateController;
        private IPlayerInputHolder _inputHolder;
        private ICameraProvider _cameraProvider;

        #endregion

        #region Properties

        public IManagerHolder ManagerHolder => _managerHolder;

        #endregion

        #region Inject

        [Inject]
        private void Construct(
            ICameraProvider cameraProvider,
            IPlayerInputHolder inputHolder,
            IConnectorRegistry connectorRegistry)
        {
            this._cameraProvider = cameraProvider;
            this._inputHolder = inputHolder;

            _managerHolder = new ManagerHolder(
                _panManager, _zoomManager, _selectionManager, _dragManager, _connectionManager);
        }

        #endregion

        #region Init

        public async UniTask InitializeAsync(PlayerInitArgs args)
        {
            _cameraProvider.SetCamera(args.Camera);
            OnCameraChanged(args.Camera);

            _panManager.SetTarget(args.PanTarget);
            _zoomManager.SetTarget(args.ZoomTarget);

            _stateController = new PlayerStateController(this, new StateMachine());

            await _stateController.ToIdle();
        }

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            if (_cameraProvider == null)
                return;

            if (subscribe) _cameraProvider.onCameraChanged += OnCameraChanged;
            else _cameraProvider.onCameraChanged -= OnCameraChanged;
        }

        private void OnCameraChanged(Camera camera)
        {
            _inputHolder.PlayerInput.camera = camera;
        }

        #endregion
    }
}
