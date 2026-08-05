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
        //[SerializeField] private SelectionManager _selectionManager;
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

        public void BeginDrag(AbstractDraggable draggable)
        {
            if (draggable == null)
                return;

            IsDragging = true;
            Current = draggable;

            RaiseOnDragStarted();
        }

        public void EndDrag()
        {
            IsDragging = false;
            RaiseOnDragCompleted();
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
            IsDragging = false;
            RaiseOnDragCompleted();
        }

        private void RaiseOnDragStarted() => onDragStarted?.Invoke(Current);
        private void RaiseOnDragCompleted() => onDragCompleted?.Invoke(Current);

        protected override void Subscribe(bool subscribe)
        {
            SubscribeToSelectionManager(subscribe);
            SubscribeToClick(subscribe);
            SubscribeToCursorDelta(subscribe);
        }

        private void SubscribeToSelectionManager(bool subscribe)
        {
            /*
            if (_selectionManager == null)
                return;

            if (subscribe)
            {
                _selectionManager.onSelected += OnSelected;
                _selectionManager.onReleased += OnReleased;
            }
            else
            {
                _selectionManager.onSelected -= OnSelected;
                _selectionManager.onReleased -= OnReleased;
            }*/
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
