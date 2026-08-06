using Main.Common.Behaviours;
using Main.Gameplay.Cameras;
using Main.Gameplay.Connectors;
using Main.Gameplay.Data;
using Main.Gameplay.Managers;
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
        private SceneData _sceneData;
        private ICameraProvider _cameraProvider;
        private ConnectorRegistry _connectorRegistry;

        #endregion

        #region Properties

        private PlayerInput PlayerInput => _inputHandler.PlayerInput;

        #endregion

        #region Inject

        [Inject]
        private void Construct(
            SceneData sceneData, 
            ICameraProvider cameraProvider,
            PlayerInputHandler inputHandler,
            ConnectorRegistry connectorRegistry)
        {
            this._sceneData = sceneData;
            this._cameraProvider = cameraProvider;
            this._inputHandler = inputHandler;
            this._connectorRegistry = connectorRegistry;

            _actionProcessor = new PlayerActionProcessor(new PlayerActionProcessorParameters(
                _connectorRegistry, _selectionManager, _panAndZoomManager, _dragManager, _connectionManager));
        }

        #endregion

        #region Unity Methods

        protected override async void Start()
        {
            base.Start();

            PlayerInput.camera = _cameraProvider.GetCamera();

            await _actionProcessor.InitializeAsync(new PlayerActionProcessorInitArgs(
                _sceneData.MainPanAndZoomTarget));
        }

        #endregion

        #region Subscribe

        
        protected override void SubscribeInner(bool subscribe)
        { }

        #endregion
    }
}
