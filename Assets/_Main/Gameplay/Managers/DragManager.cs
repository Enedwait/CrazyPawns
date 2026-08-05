using Main.Common.Behaviours;
using Main.Gameplay.Cameras;
using Main.Infrastructure.Controls.Providers;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Main.Gameplay.Managers
{
    [DisallowMultipleComponent]
    public sealed class DragManager : AbstractMonoBehaviourExtended
    {
        [SerializeField] private ClickProvider _clickProvider;
        [SerializeField] private Vector2DeltaProvider _cursorDeltaProvider;
        [SerializeField] private CursorPositionProvider _cursorPositionProvider;

        private ICameraProvider _cameraProvider;
        private Camera Camera => _cameraProvider.GetCamera();

        public bool IsActive { get; private set; }
        public bool IsDragging { get; private set; }
        public AbstractDraggable Current { get; private set; }

        public event UnityAction<AbstractDraggable> onDragStarted;
        public event UnityAction<AbstractDraggable> onDragCompleted;

        [Inject]
        private void Construct(ICameraProvider cameraProvider)
        {
            this._cameraProvider = cameraProvider;
        }

        public void SetActive(bool active)
        {
            IsActive = active;
        }

        public bool BeginDrag(AbstractDraggable draggable)
        {
            if (draggable == null)
                return false;

            if (!draggable.BeginDrag())
                return false;

            IsDragging = true;
            Current = draggable;

            RaiseOnDragStarted(draggable);
            return true;
        }

        public bool EndDrag()
        {
            var draggable = Current;
            if (draggable == null)
                return true;

            if (!draggable.EndDrag())
                return false;

            IsDragging = false;
            Current = null;

            RaiseOnDragCompleted(draggable);
            return true;
        }

        private void OnSelected(AbstractSelectable selected)
        {
            BeginDrag(selected.GetComponent<AbstractDraggable>());
        }
        
        private void OnReleased(AbstractSelectable released)
        {
            EndDrag();
        }

        private void OnClick()
        { }

        private void OnClickCanceled()
        {
            EndDrag();
        }

        private void RaiseOnDragStarted(AbstractDraggable draggable) => onDragStarted?.Invoke(draggable);
        private void RaiseOnDragCompleted(AbstractDraggable draggable) => onDragCompleted?.Invoke(draggable);

        protected override void SubscribeInner(bool subscribe)
        {
            SubscribeToClick(subscribe);
            SubscribeToCursorDelta(subscribe);
        }

        private void SubscribeToClick(bool subscribe)
        {
            if (_clickProvider == null)
                return;

            if (subscribe)
            {
                _clickProvider.onClickPerformed += OnClick;
                _clickProvider.onClickCanceled += OnClickCanceled;
            }
            else
            {
                _clickProvider.onClickPerformed -= OnClick;
                _clickProvider.onClickCanceled -= OnClickCanceled;
            }
        }

        private void SubscribeToCursorDelta(bool subscribe)
        {
            if (_cursorDeltaProvider == null)
                return;

            if (subscribe)
            {
                _cursorDeltaProvider.onDelta += OnCursorDelta;
            }
            else
            {
                _cursorDeltaProvider.onDelta -= OnCursorDelta;
            }
        }

        private void OnCursorDelta(Vector2 delta)
        {
            if (!IsActive || !IsDragging)
                return;

            Vector3 worldPosition = _cursorPositionProvider.GetWorldPositionWithY(Camera, 0f);
            Vector3 direction = (worldPosition - Current.Target.position);

            Current.Drag(direction);
        }

        protected override void OnDestroy()
        {
            SetActive(false);
            base.OnDestroy();
        }
    }
}
