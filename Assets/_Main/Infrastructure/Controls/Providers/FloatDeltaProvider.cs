using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Main.Infrastructure.Controls.Providers
{
    public class FloatDeltaProvider : AbstractInputProvider
    {
        [SerializeField] private InputActionReference _floatDeltaActionReference;

        private InputAction _deltaAction;

        public event UnityAction<float> onDelta;

        public float Delta { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            _deltaAction = _floatDeltaActionReference.action;

            ResetValues();
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

        private void RaiseOnZoom() => 
            onDelta?.Invoke(Delta);

        public override void ResetValues() =>
            Delta = 0f;

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
    }
}
