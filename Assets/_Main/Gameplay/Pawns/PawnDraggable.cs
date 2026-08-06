using Main.Gameplay.Checkerboards;
using Main.Gameplay.Data;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Main.Common.Behaviours
{
    public class PawnDraggable : AbstractDraggable, IPawnDraggable
    {
        #region Fields

        private bool isInsideCheckerBoard = true;
        private SceneData _sceneData;

        #endregion

        #region Properties

        private Checkerboard Checkerboard => _sceneData.Checkerboard;

        #endregion

        #region Events

        public UnityAction<PawnEnterCheckerboardEventArgs> onEnterCheckerboard;
        public UnityAction<PawnExitCheckerboardEventArgs> onExitCheckerboard;
        public UnityAction<PawnDragEndedOutsideEventArgs> onDragEndedOutside;

        #endregion

        #region Inject

        [Inject]
        private void Construct(SceneData sceneData)
        {
            this._sceneData = sceneData;
        }

        #endregion

        #region BeginDrag

        protected override bool BeginDragInner()
        {
            return true;
        }

        protected override void DragInner(Vector3 direction)
        {
            Target.Translate(direction, Space.World);
            CheckCheckerboard(Checkerboard);
        }

        private void CheckCheckerboard(ICheckerboard checkerboard)
        {
            if (checkerboard == null)
                return;

            if (checkerboard.IsInside(Target.position))
            {
                if (!isInsideCheckerBoard)
                {
                    isInsideCheckerBoard = true;
                    RaiseOnEnterCheckerboard(Checkerboard);
                }
            }
            else
            {
                if (isInsideCheckerBoard)
                {
                    isInsideCheckerBoard = false;
                    RaiseOnExitCheckerboard(Checkerboard);
                }
            }
        }

        #endregion

        #region EndDrag

        protected override bool EndDragInner()
        {
            if (isInsideCheckerBoard)
            { }
            else 
                RaiseOnDragEndedOutside(Checkerboard);

            return true;
        }

        #endregion

        #region Event Raisers

        protected void RaiseOnEnterCheckerboard(ICheckerboard checkerboard) =>
            onEnterCheckerboard?.Invoke(new PawnEnterCheckerboardEventArgs(this, checkerboard));

        protected void RaiseOnExitCheckerboard(ICheckerboard checkerboard) =>
            onExitCheckerboard?.Invoke(new PawnExitCheckerboardEventArgs(this, checkerboard));

        protected void RaiseOnDragEndedOutside(ICheckerboard checkerboard) =>
            onDragEndedOutside?.Invoke(new PawnDragEndedOutsideEventArgs(this, checkerboard));

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        { }

        #endregion
    }

    public interface IPawnDraggable : IDraggable
    { }

    public record PawnEnterCheckerboardEventArgs(IPawnDraggable Draggable, ICheckerboard Checkerboard);
    public record PawnExitCheckerboardEventArgs(IPawnDraggable Draggable, ICheckerboard Checkerboard);
    public record PawnDragEndedOutsideEventArgs(IPawnDraggable Draggable, ICheckerboard Checkerboard);
}
