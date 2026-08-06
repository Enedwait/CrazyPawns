using UnityEngine;

namespace Main.Common.Behaviours
{
    public abstract class AbstractDraggable : AbstractMonoBehaviourExtended, IDraggable
    {
        #region Fields

        [SerializeField] protected Transform target;

        #endregion

        #region Properties

        public Transform Target => target;
        public bool IsDragging { get; protected set; }
        public bool CanDrag { get; protected set; }
        public Vector3 Position => 
            target != null ? target.position : 
            transform != null ? transform.position : Vector3.zero;

        #endregion

        #region Unity Methods

        protected override void Start()
        {
            base.Start();

            CanDrag = true;
        }

        #endregion

        #region BeginDrag

        public bool BeginDrag()
        {
            if (!CanDrag && IsDragging) return false;
            if (!BeginDragInner()) return false;

            IsDragging = true;
            return true;
        }

        protected abstract bool BeginDragInner();

        #endregion

        #region Drag

        public void Drag(Vector3 direction)
        {
            if (!CanDrag && IsDragging)
                return;

            DragInner(direction);
        }

        protected abstract void DragInner(Vector3 direction);

        #endregion

        #region EndDrag

        public bool EndDrag()
        {
            if (!IsDragging) return true;
            if (!EndDragInner()) return false;
            
            IsDragging = false;
            return true;
        }

        protected abstract bool EndDragInner();

        #endregion
    }

    public interface IDraggable
    {
        Vector3 Position { get; }

        bool BeginDrag();
        void Drag(Vector3 direction);
        bool EndDrag();
    }
}
