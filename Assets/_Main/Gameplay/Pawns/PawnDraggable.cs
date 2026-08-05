using Main.Gameplay.Checkerboards;
using UnityEngine;
using UnityEngine.Events;

namespace Main.Common.Behaviours
{
    public class PawnDraggable : AbstractDraggable
    {
        public UnityAction<PawnEnterCheckerboardEventArgs> onEnterCheckerboard;
        public UnityAction<PawnExitCheckerboardEventArgs> onExitCheckerboard;
        public UnityAction<PawnDragEndedOutsideEventArgs> onDragEndedOutside;

        private bool isInsideCheckerBoard;
        private Checkerboard Checkerboard => sceneData.Checkerboard;

        protected override bool BeginDragInner()
        {
            return true;
        }

        protected override void DragInner(Vector3 direction)
        {
            Target.Translate(direction, Space.World);
            CheckCheckerboard(Checkerboard);
        }

        private void CheckCheckerboard(Checkerboard checkerboard)
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

        protected override bool EndDragInner()
        {
            if (isInsideCheckerBoard)
            { }
            else 
                RaiseOnDragEndedOutside(Checkerboard);

            return true;
        }

        protected void RaiseOnEnterCheckerboard(Checkerboard checkerboard) =>
            onEnterCheckerboard?.Invoke(new PawnEnterCheckerboardEventArgs(this, checkerboard));

        protected void RaiseOnExitCheckerboard(Checkerboard checkerboard) =>
            onExitCheckerboard?.Invoke(new PawnExitCheckerboardEventArgs(this, checkerboard));

        protected void RaiseOnDragEndedOutside(Checkerboard checkerboard) =>
            onDragEndedOutside?.Invoke(new PawnDragEndedOutsideEventArgs(this, checkerboard));

        protected override void SubscribeInner(bool subscribe)
        { }
    }

    public record PawnEnterCheckerboardEventArgs(PawnDraggable Draggable, Checkerboard Checkerboard);
    public record PawnExitCheckerboardEventArgs(PawnDraggable Draggable, Checkerboard Checkerboard);
    public record PawnDragEndedOutsideEventArgs(PawnDraggable Draggable, Checkerboard Checkerboard);
}
