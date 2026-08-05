using UnityEngine;

namespace Main.Common.Behaviours
{
    public class PawnDraggable : AbstractDraggable
    {
        protected override void BeginDragInner()
        { }

        protected override void DragInner(Vector3 direction)
        {
            Target.Translate(direction, Space.World);
        }

        protected override void EndDragInner()
        { }
    }
}
