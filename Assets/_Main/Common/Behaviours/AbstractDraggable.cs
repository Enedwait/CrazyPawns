using Main.Gameplay.Data;
using UnityEngine;
using Zenject;

namespace Main.Common.Behaviours
{
    public abstract class AbstractDraggable : AbstractMonoBehaviourExtended
    {
        [SerializeField] protected Transform target;

        protected SceneData sceneData;

        public Transform Target => target;
        public bool IsDragging { get; protected set; }
        public bool CanDrag { get; protected set; }

        [Inject]
        private void Construct(SceneData sceneData)
        {
            this.sceneData = sceneData;
        }

        protected override void Start()
        {
            base.Start();

            CanDrag = true;
        }

        public bool BeginDrag()
        {
            if (!CanDrag && IsDragging)
                return false;

            if (BeginDragInner())
            {
                IsDragging = true;
                return true;
            }

            return false;
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
            if (!IsDragging)
                return true;

            if (!EndDragInner())
                return false;
            
            IsDragging = false;
            return true;
        }

        protected abstract bool EndDragInner();
    }
}
