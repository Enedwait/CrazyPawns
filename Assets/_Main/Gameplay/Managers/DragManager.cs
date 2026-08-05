using Main.Common.Behaviours;
using Main.Gameplay.Cameras;
using Main.Gameplay.Players;
using Main.Infrastructure.Controls.Providers;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Main.Gameplay.Managers
{
    [DisallowMultipleComponent]
    public sealed class DragManager : AbstractManager
    {
        private ClickProvider _clickProvider;
        private Vector2DeltaProvider _cursorDeltaProvider;
        private CursorPositionProvider _cursorPositionProvider;
        private ICameraProvider _cameraProvider;
        private Camera Camera => _cameraProvider.GetCamera();
        
        public bool IsDragging { get; private set; }
        public AbstractDraggable Current { get; private set; }

        public event UnityAction<DragStartedEventArgs> onDragStarted;
        public event UnityAction<DragEndedEventArgs> onDragCompleted;

        [Inject]
        private void Construct(
            ICameraProvider cameraProvider,
            PlayerInputHandler inputHandler)
        {
            this._cameraProvider = cameraProvider;
            this._clickProvider = inputHandler.ClickProvider;
            this._cursorPositionProvider = inputHandler.CursorPositionProvider;
            this._cursorDeltaProvider = inputHandler.CursorDeltaProvider;
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

        #region Event Raisers

        private void RaiseOnDragStarted(AbstractDraggable draggable) => 
            onDragStarted?.Invoke(new DragStartedEventArgs(this, draggable));

        private void RaiseOnDragCompleted(AbstractDraggable draggable) => 
            onDragCompleted?.Invoke(new DragEndedEventArgs(this, draggable));

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            SubscribeToClick(subscribe);
            SubscribeToCursorDelta(subscribe);
        }

        #endregion

        #region SubscribeToClick

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

        private void OnClick()
        { }

        private void OnClickCanceled()
        {
            EndDrag();
        }

        #endregion

        #region SubscribeToCursorDelta

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

        #endregion
    }

    public record DragStartedEventArgs(DragManager DragManager, AbstractDraggable Draggable);
    public record DragEndedEventArgs(DragManager DragManager, AbstractDraggable Draggable);
}
