using UnityEngine;

namespace Main.Common.Behaviours
{
    public abstract class AbstractDraggable : AbstractMonoBehaviourExtended, IDraggable
    {
        [SerializeField] protected Transform target;

        public Transform Target => target;
        public bool IsDragging { get; protected set; }
        public bool CanDrag { get; protected set; }
        public Vector3 Position => 
            target != null ? target.position : 
            transform != null ? transform.position : Vector3.zero;

        protected override void Start()
        {
            base.Start();

            CanDrag = true;
        }

        public bool BeginDrag()
        {
            if (!CanDrag && IsDragging) return false;
            if (!BeginDragInner()) return false;

            IsDragging = true;
            return true;
        }

        protected abstract bool BeginDragInner();

        public void Drag(Vector3 direction)
        {
            if (!CanDrag && IsDragging)
                return;

            DragInner(direction);
        }

        protected abstract void DragInner(Vector3 direction);

        public bool EndDrag()
        {
            if (!IsDragging) return true;
            if (!EndDragInner()) return false;
            
            IsDragging = false;
            return true;
        }

        protected abstract bool EndDragInner();
    }

    public interface IDraggable
    {
        Vector3 Position { get; }

        bool BeginDrag();
        void Drag(Vector3 direction);
        bool EndDrag();
    }
}
