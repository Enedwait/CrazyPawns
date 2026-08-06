using Cysharp.Threading.Tasks;
using Main.Common.Behaviours;
using Main.Gameplay.Connectors;
using Main.Gameplay.Managers.Connection;
using Main.Gameplay.Managers.Drag;
using Main.Gameplay.Managers.PanAndZoom;
using Main.Gameplay.Managers.Selection;
using Main.Gameplay.Targets;
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
        [SerializeField] private PanAndZoomManager _panAndZoomManager;
        [SerializeField] private DragManager _dragManager;
        [SerializeField] private ConnectionManager _connectionManager;

        #endregion

        #region Fields

        private PlayerActionProcessor _actionProcessor;
        private PlayerInputHandler _inputHandler;
        private ICameraProvider _cameraProvider;

        #endregion

        #region Properties

        private PlayerInput PlayerInput => _inputHandler.PlayerInput;

        #endregion

        #region Inject

        [Inject]
        private void Construct(
            ICameraProvider cameraProvider,
            PlayerInputHandler inputHandler,
            ConnectorRegistry connectorRegistry)
        {
            this._cameraProvider = cameraProvider;
            this._inputHandler = inputHandler;

            _actionProcessor = new PlayerActionProcessor(new PlayerActionProcessorParameters(
               _selectionManager, _panAndZoomManager, _dragManager, _connectionManager));
        }

        #endregion

        #region Init

        public async UniTask InitializeAsync(PlayerInitArgs args)
        {
            _cameraProvider.SetCamera(args.Camera);
            PlayerInput.camera = _cameraProvider.GetCamera();

            await _actionProcessor.InitializeAsync(new PlayerActionProcessorInitArgs(
                args.PanAndZoomTarget));
        }

        #endregion

        #region Subscribe


        protected override void SubscribeInner(bool subscribe)
        { }

        #endregion
    }

    public record PlayerInitArgs(Camera Camera, PanAndZoomTarget PanAndZoomTarget);
}
