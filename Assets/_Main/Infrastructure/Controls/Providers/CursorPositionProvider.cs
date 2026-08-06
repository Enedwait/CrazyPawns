using Main.Common.Extensions;
using Main.Common.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.Infrastructure.Controls.Providers
{
    public sealed class CursorPositionProvider : AbstractInputProvider, ICursorPositionProvider
    {
        #region Fields

        [SerializeField] private bool _useScreenCenter = true;
        [SerializeField] private Vector2 _initialPosition = Vector2.zero;
        [SerializeField] private InputActionReference _positionActionReference;

        private InputAction _positionAction;
        private Vector2 _cursorPosition;

        #endregion

        #region Properties

        public Vector2 CursorPosition => _cursorPosition;

        #endregion

        #region Unity Methods

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (_useScreenCenter)
                _initialPosition = ScreenHelper.GetScreenCenter();
        }
#endif

        protected override void Awake()
        {
            base.Awake();

            if (_useScreenCenter)
                _initialPosition = ScreenHelper.GetScreenCenter();

            _positionAction = _positionActionReference.action;

            ResetValues();
        }

        #endregion

        #region Methods

        public Ray GetCameraRay(Camera camera) =>
            camera.ScreenPointToRay(CursorPosition);

        public Vector3 GetWorldPositionWithY(Camera camera, float y = 0f) =>
            GetCameraRay(camera).GetPointAlongRayWithY(y);

        public override void ResetValues() =>
            _cursorPosition = _initialPosition;

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            if (_positionAction == null)
                return;

            if (subscribe)
                _positionAction.performed += OnPositionPerformed;
            else
                _positionAction.performed -= OnPositionPerformed;
        }

        private void OnPositionPerformed(InputAction.CallbackContext context) =>
            _cursorPosition = context.ReadValue<Vector2>();

        #endregion
    }
}
