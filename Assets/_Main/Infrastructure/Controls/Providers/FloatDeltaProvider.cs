using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Main.Infrastructure.Controls.Providers
{
    public sealed class FloatDeltaProvider : AbstractInputProvider
    {
        #region Fields

        [SerializeField] private InputActionReference _floatDeltaActionReference;

        private InputAction _deltaAction;

        #endregion

        #region Properties

        public float Delta { get; private set; }

        #endregion

        #region Events

        public event UnityAction<float> onDelta;

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            _deltaAction = _floatDeltaActionReference.action;

            ResetValues();
        }

        #endregion

        #region Methods


        private void RaiseOnZoom() => 
            onDelta?.Invoke(Delta);

        public override void ResetValues() =>
            Delta = 0f;

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
            Delta = _deltaAction.ReadValue<float>();
            RaiseOnZoom();
        }

        private void OnDeltaCanceled(InputAction.CallbackContext context)
        {
            ResetValues();
            RaiseOnZoom();
        }
        
        #endregion
    }
}
