using UnityEngine;

namespace Main.Common.Behaviours
{
    public abstract class AbstractDraggable : MonoBehaviour
    {
        [SerializeField] protected Transform target;

        public Transform Target => target;
        public bool IsDragging { get; protected set; }
        public bool CanDrag { get; protected set; }

        public void BeginDrag()
        {
            if (!CanDrag && IsDragging)
                return;

            BeginDragInner();
            IsDragging = true;
        }

        protected abstract void BeginDragInner();

        public void Drag(Vector3 direction)
        {
            if (!CanDrag && IsDragging)
                return;

            DragInner(direction);
        }

        protected abstract void DragInner(Vector3 direction);

        public void EndDrag()
        {
            if (!IsDragging)
                return;

            EndDragInner();
            IsDragging = false;
        }

        protected abstract void EndDragInner();
    }
}
