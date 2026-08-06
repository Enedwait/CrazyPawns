using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Main.Infrastructure.Controls.Providers
{
    public sealed class Vector2DeltaProvider : AbstractInputProvider, IVector2DeltaProvider
    {
        #region Fields

        [SerializeField] private InputActionReference _vector2DeltaActionReference;

        private InputAction _deltaAction;

        #endregion

        #region Properties

        public Vector2 Delta { get; private set; }

        #endregion

        #region Events

        public event UnityAction<Vector2> onDelta;

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            _deltaAction = _vector2DeltaActionReference.action;

            ResetValues();
        }

        #endregion

        #region Methods

        private void RaiseOnDelta() => 
            onDelta?.Invoke(Delta);

        public override void ResetValues() =>
            Delta = Vector2.zero;

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            if (_deltaAction == null)
                return;

            if (subscribe)
            {
                _deltaAction.performed += OnDeltaPerformed;
                _deltaAction.canceled += OnDeltaCanceled;
            }
            else
            {
                _deltaAction.performed -= OnDeltaPerformed;
                _deltaAction.canceled -= OnDeltaCanceled;
            }
        }

        private void OnDeltaPerformed(InputAction.CallbackContext context)
        {
            Delta = _deltaAction.ReadValue<Vector2>();
            RaiseOnDelta();
        }

        private void OnDeltaCanceled(InputAction.CallbackContext context)
        {
            ResetValues();
            RaiseOnDelta();
        }

        #endregion
    }

    
}
