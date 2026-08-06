using Main.Common.Behaviours;
using Main.Common.Extensions;
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
        #region Fields

        private ClickProvider _clickProvider;
        private Vector2DeltaProvider _cursorDeltaProvider;
        private CursorPositionProvider _cursorPositionProvider;
        private ICameraProvider _cameraProvider;

        #endregion

        #region Properties

        private Camera Camera => _cameraProvider.GetCamera();

        public bool IsDragging { get; private set; }
        public IDraggable Current { get; private set; }

        #endregion

        #region Events

        public event UnityAction<DragStartedEventArgs> onDragStarted;
        public event UnityAction<DragEndedEventArgs> onDragCompleted;

        #endregion

        #region Inject

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

        #endregion

        #region BeginDrag

        public bool BeginDrag(IDraggable draggable)
        {
            if (!IsActive)
                return false;

            if (draggable.IsNullOrDestroyed())
                return false;

            if (!draggable.BeginDrag())
                return false;

            IsDragging = true;
            Current = draggable;

            RaiseOnDragStarted(draggable);
            return true;
        }

        #endregion

        #region EndDrag

        public bool EndDrag()
        {
            if (!IsActive)
                return false;

            var draggable = Current;
            if (draggable == null)
                return true;

            if (!draggable.EndDrag())
                return false;

            FinalizeEndDrag(draggable);
            return true;
        }

        private void FinalizeEndDrag(IDraggable draggable)
        {
            IsDragging = false;
            Current = null;

            RaiseOnDragCompleted(draggable);
        }

        #endregion

        #region Event Raisers

        private void RaiseOnDragStarted(IDraggable draggable) => 
            onDragStarted?.Invoke(new DragStartedEventArgs(this, draggable));

        private void RaiseOnDragCompleted(IDraggable draggable) => 
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
                _clickProvider.onClickCanceled += OnClickCanceled;
            }
            else
            {
                _clickProvider.onClickCanceled -= OnClickCanceled;
            }
        }

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

            if (Current.IsNullOrDestroyed())
            {
                EndDrag();
                return;
            }

            Vector3 worldPosition = _cursorPositionProvider.GetWorldPositionWithY(Camera, 0f);
            Vector3 direction = (worldPosition - Current.Position);

            Current.Drag(direction);
        }

        #endregion
    }

    public record DragStartedEventArgs(DragManager DragManager, IDraggable Draggable);
    public record DragEndedEventArgs(DragManager DragManager, IDraggable Draggable);
}
