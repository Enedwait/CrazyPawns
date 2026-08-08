using Cysharp.Threading.Tasks;
using Main.Common.Behaviours;
using Main.Gameplay.Connectors;
using Main.Gameplay.Managers.Connection;
using Main.Gameplay.Managers.Drag;
using Main.Gameplay.Managers.Pan;
using Main.Gameplay.Managers.Selection;
using Main.Gameplay.Managers.Zoom;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Main.Gameplay.Players
{
    [DisallowMultipleComponent]
    public sealed class Player : AbstractMonoBehaviourExtended
    {
        #region Serialize Field

        [SerializeField] private SelectionManager _selectionManager;
        [SerializeField] private PanManager _panManager;
        [SerializeField] private ZoomManager _zoomManager;
        [SerializeField] private DragManager _dragManager;
        [SerializeField] private ConnectionManager _connectionManager;

        #endregion

        #region Fields

        private IPlayerActionProcessor _actionProcessor;
        private IPlayerInputHolder _inputHolder;
        private ICameraProvider _cameraProvider;

        #endregion

        #region Properties

        private PlayerInput PlayerInput => _inputHolder.PlayerInput;

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

            _actionProcessor = new PlayerActionProcessor(new PlayerActionProcessorParameters(
               _selectionManager, _panManager, _zoomManager, _dragManager, _connectionManager));
        }

        #endregion

        #region Init

        public async UniTask InitializeAsync(PlayerInitArgs args)
        {
            _cameraProvider.SetCamera(args.Camera);
            PlayerInput.camera = _cameraProvider.GetCamera();

            await _actionProcessor.InitializeAsync(new PlayerActionProcessorInitArgs(
                args.PanTarget, args.ZoomTarget));
        }

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        { }

        #endregion
    }
}
