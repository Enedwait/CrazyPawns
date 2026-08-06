using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Main.Infrastructure.Controls.Providers
{
    public sealed class ClickProvider : AbstractInputProvider
    {
        #region Fields

        [SerializeField] private InputActionReference _clickActionReference;

        private InputAction clickAction;

        #endregion

        #region Events

        public event UnityAction onClickStarted;
        public event UnityAction onClickPerformed;
        public event UnityAction onClickCanceled;

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            clickAction = _clickActionReference.action;

            ResetValues();
        }

        #endregion

        #region Event Raisers

        private void RaiseOnClickStarted() => onClickStarted?.Invoke();
        private void RaiseOnClickPerformed() => onClickPerformed?.Invoke();
        private void RaiseOnClickCanceled() => onClickCanceled?.Invoke();

        #endregion

        #region Reset

        public override void ResetValues()
        { }

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
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

        private void OnClickStarted(InputAction.CallbackContext context) =>
            RaiseOnClickStarted();

        private void OnClickPerformed(InputAction.CallbackContext context) =>
            RaiseOnClickPerformed();

        private void OnClickCanceled(InputAction.CallbackContext context) =>
            RaiseOnClickCanceled();

        #endregion
    }
}
