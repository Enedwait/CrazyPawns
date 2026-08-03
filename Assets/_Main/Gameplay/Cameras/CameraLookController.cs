using Main.Common;
using Main.Gameplay.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Main.Gameplay.Cameras
{
    public sealed class CameraLookController : AbstractMonoBehaviourExtended
    {
        #region Serialize Fields

        [SerializeField, Header("Cursor")] private InputActionReference _cursorPositionActionReference;

        [SerializeField, Header("Pan")] private InputActionReference _panActionReference;
        [SerializeField, Range(0.001f, 10f)] private float _lookSensitivity = 1.75f;

        [SerializeField, Header("Zoom")] private InputActionReference _zoomActionReference;
        [SerializeField, Range(0.001f, 10f)] private float _zoomSensitivity = 0.27f;

        #endregion

        #region Fields

        private SceneData _sceneData;

        private InputAction _cursorPositionAction;

        private InputAction _panAction;
        private Vector2 _moveDelta;
        private bool _isMoveActive;

        private InputAction _zoomAction;
        private float _zoomDelta;
        private bool _isZoomActive;

        #endregion

        #region Properties

        private Camera Camera => _sceneData.MainCamera;

        #endregion

        #region Inject

        [Inject]
        private void Construct(SceneData sceneData)
        {
            this._sceneData = sceneData;
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _cursorPositionAction = _cursorPositionActionReference.action;
            _panAction = _panActionReference.action;
            _zoomAction = _zoomActionReference.action;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            PanCamera(in deltaTime);
            ZoomCamera(in deltaTime);
        }

        #endregion

        #region Pan

        private void PanCamera(in float deltaTime)
        {
            if (!_isMoveActive)
                return;

            Camera.transform.Translate(
                new Vector3(_moveDelta.x, 0, _moveDelta.y) * _lookSensitivity * deltaTime, 
                Space.World);
        }

        private void OnPanPerformed(InputAction.CallbackContext context)
        {
            _moveDelta = context.ReadValue<Vector2>();
            _isMoveActive = true;
        }

        private void OnPanCanceled(InputAction.CallbackContext context)
        {
            _moveDelta = Vector2.zero;
            _isMoveActive = false;
        }

        #endregion

        #region Zoom

        private void ZoomCamera(in float deltaTime)
        {
            if (!_isZoomActive)
                return;

            Vector2 cursorScreenPosition = _cursorPositionAction.ReadValue<Vector2>();
            Ray cameraRay = Camera.ScreenPointToRay(cursorScreenPosition);

            Camera.transform.Translate(
                cameraRay.direction * _zoomDelta * _zoomSensitivity * deltaTime, 
                Space.World);
        }

        private void OnZoomPerformed(InputAction.CallbackContext context)
        {
            _zoomDelta = context.ReadValue<float>();
            _isZoomActive = true;
        }

        private void OnZoomCanceled(InputAction.CallbackContext context)
        {
            _zoomDelta = 0f;
            _isZoomActive = false;
        }

        #endregion

        #region Subscribe

        protected override void Subscribe(bool subscribe)
        {
            if (subscribe)
            {
                if (_panAction != null)
                {
                    _panAction.performed += OnPanPerformed;
                    _panAction.canceled += OnPanCanceled;
                }

                if (_zoomAction != null)
                {
                    _zoomAction.performed += OnZoomPerformed;
                    _zoomAction.canceled += OnZoomCanceled;
                }
            }
            else
            {
                if (_panAction != null)
                {
                    _panAction.performed -= OnPanPerformed;
                    _panAction.canceled -= OnPanCanceled;
                }

                if (_zoomAction != null)
                {
                    _zoomAction.performed -= OnZoomPerformed;
                    _zoomAction.canceled -= OnZoomCanceled;
                }
            }
        }

        #endregion
    }
}
