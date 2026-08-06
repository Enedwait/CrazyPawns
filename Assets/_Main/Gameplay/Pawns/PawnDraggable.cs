using Main.Common.Behaviours;
using Main.Gameplay.Checkerboards;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Main.Gameplay.Pawns
{
    public class PawnDraggable : AbstractDraggable, IPawnDraggable
    {
        #region Fields

        private bool isInsideCheckerBoard = true;
        private ICheckerboard _checkerboard;

        #endregion

        #region Events

        public event UnityAction<PawnEnterCheckerboardEventArgs> onEnterCheckerboard;
        public event UnityAction<PawnExitCheckerboardEventArgs> onExitCheckerboard;
        public event UnityAction<PawnDragEndedOutsideEventArgs> onDragEndedOutside;

        #endregion

        #region Inject

        [Inject]
        private void Construct(ICheckerboard checkerboard)
        {
            this._checkerboard = checkerboard;
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
            CheckCheckerboard(_checkerboard);
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
                    RaiseOnEnterCheckerboard(_checkerboard);
                }
            }
            else
            {
                if (isInsideCheckerBoard)
                {
                    isInsideCheckerBoard = false;
                    RaiseOnExitCheckerboard(_checkerboard);
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
                RaiseOnDragEndedOutside(_checkerboard);

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
}
