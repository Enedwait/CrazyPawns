using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Main.Infrastructure.Controls.Providers
{
    public sealed class ClickProvider : AbstractInputProvider
    {
        [SerializeField] private InputActionReference _clickActionReference;

        private InputAction clickAction;

        public event UnityAction onClickStarted;
        public event UnityAction onClickPerformed;
        public event UnityAction onClickCanceled;

        private void Awake()
        {
            clickAction = _clickActionReference.action;

            ResetValues();
        }

        private void OnClickStarted(InputAction.CallbackContext context) =>
            RaiseOnClickStarted();

        private void OnClickPerformed(InputAction.CallbackContext context) =>
            RaiseOnClickPerformed();

        private void OnClickCanceled(InputAction.CallbackContext context) =>
            RaiseOnClickCanceled();

        private void RaiseOnClickStarted() => onClickStarted?.Invoke();
        private void RaiseOnClickPerformed() => onClickPerformed?.Invoke();
        private void RaiseOnClickCanceled() => onClickCanceled?.Invoke();

        public override void ResetValues()
        { }

        protected override void Subscribe(bool subscribe)
        {
            if (clickAction == null)
                return;

            if (subscribe)
            {
                clickAction.started += OnClickStarted;
                clickAction.performed += OnClickPerformed;
                clickAction.canceled += OnClickCanceled;
            }
            else
            {
                clickAction.started -= OnClickStarted;
                clickAction.performed -= OnClickPerformed;
                clickAction.canceled -= OnClickCanceled;
            }
        }
    }
}
