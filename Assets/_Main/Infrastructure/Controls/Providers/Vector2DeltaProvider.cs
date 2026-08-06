using Main.Common.Behaviours;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Main.Infrastructure.Controls.Providers
{
    public class Vector2DeltaProvider : AbstractInputProvider
    {
        [SerializeField] private InputActionReference _vector2DeltaActionReference;

        private InputAction _deltaAction;

        public event UnityAction<Vector2> onDelta;

        public Vector2 Delta { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            _deltaAction = _vector2DeltaActionReference.action;

            ResetValues();
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

        private void RaiseOnDelta() => 
            onDelta?.Invoke(Delta);

        public override void ResetValues() =>
            Delta = Vector2.zero;

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

    public abstract class AbstractInputProvider : AbstractMonoBehaviourExtended
    {
        public abstract void ResetValues();
    }
}
