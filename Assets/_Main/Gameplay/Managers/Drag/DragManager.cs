using System;
using Cysharp.Threading.Tasks;
using Main.Common.Behaviours;
using Main.Common.Extensions;
using Main.Gameplay.Players;
using Main.Infrastructure.Controls.Providers;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Main.Gameplay.Managers.Drag
{
    [DisallowMultipleComponent]
    public sealed class DragManager : AbstractManager, IDragManager
    {
        #region Fields

        [SerializeField] private float _smoothTime = 0.15f;

        private CancellationTokenSource _cts;
        private Vector3 _targetPosition;

        private IClickProvider _clickProvider;
        private IVector2DeltaProvider _cursorDeltaProvider;
        private ICursorPositionProvider _cursorPositionProvider;
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
            IPlayerInputHolder inputHolder)
        {
            this._cameraProvider = cameraProvider;
            this._clickProvider = inputHolder.ClickProvider;
            this._cursorPositionProvider = inputHolder.CursorPositionProvider;
            this._cursorDeltaProvider = inputHolder.CursorDeltaProvider;
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

            if (_cts != null)
            {
                _cts.Cancel();
                _cts.Dispose();
                _cts = null;
            }

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

            _targetPosition = _cursorPositionProvider.GetWorldPositionWithY(Camera, 0f);

            if (_cts == null)
            {
                _cts = new CancellationTokenSource();
                SmoothFollowAsync(Current, _smoothTime, _cts.Token).Forget();
            }
        }

        private async UniTaskVoid SmoothFollowAsync(
            IDraggable draggable, 
            float smoothTime = 0.15f, 
            CancellationToken token = default)
        {
            Vector3 velocity = Vector3.zero;
            Vector3 currentPosition = draggable.Position;

            while (!token.IsCancellationRequested)
            {
                currentPosition = Vector3.SmoothDamp(
                    currentPosition,
                    _targetPosition,
                    ref velocity,
                    smoothTime,
                    Mathf.Infinity,
                    Time.deltaTime);

                Current.Drag(currentPosition - draggable.Position);

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }
        }

        #endregion
    }
}
