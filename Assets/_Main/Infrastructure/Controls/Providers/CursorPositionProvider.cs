using Main.Common.Extensions;
using Main.Common.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.Infrastructure.Controls.Providers
{
    public class CursorPositionProvider : AbstractInputProvider
    {
        [SerializeField] protected bool useScreenCenter;
        [SerializeField] protected Vector2 initialCursorPosition = Vector2.zero;

        [SerializeField] protected InputActionReference cursorPositionActionReference;

        protected InputAction positionAction;

        protected Vector2 cursorPosition;

        public Vector2 CursorPosition => cursorPosition;

        #region Unity Methods

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (useScreenCenter)
                initialCursorPosition = ScreenHelper.GetScreenCenter();
        }
#endif

        protected void Awake()
        {
            if (useScreenCenter)
                initialCursorPosition = ScreenHelper.GetScreenCenter();

            positionAction = cursorPositionActionReference.action;

            ResetValues();
        }

        #endregion

        #region Methods

        public Ray GetCameraRay(Camera camera) =>
            camera.ScreenPointToRay(CursorPosition);

        public Vector3 GetWorldPositionWithY(Camera camera, float y = 0f) =>
            GetCameraRay(camera).GetPointAlongRayWithY(y);

        public override void ResetValues() =>
            cursorPosition = initialCursorPosition;

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            if (positionAction == null)
                return;

            if (subscribe)
                positionAction.performed += OnPositionPerformed;
            else
                positionAction.performed -= OnPositionPerformed;
        }

        private void OnPositionPerformed(InputAction.CallbackContext context) =>
            cursorPosition = context.ReadValue<Vector2>();

        #endregion
    }
}
