using UnityEngine;
using UnityEngine.Events;

namespace Main.Common.Behaviours
{
    public abstract class AbstractSelectable : AbstractMonoBehaviourExtended
    {
        [SerializeField] protected Transform target;
        
        public Transform Target => target;
        public bool IsSelected { get; protected set; }
        public bool CanSelect { get; protected set; }

        public event UnityAction<SelectedChangedEventArgs> onSelectedChanged;

        protected override void InitComponents()
        {
            base.InitComponents();

            if (target == null)
                target = transform;
        }

        protected override void Start()
        {
            base.Start();

            CanSelect = true;
        }

        public bool Select()
        {
            if (!CanSelect || IsSelected)
                return false;

            if (!SelectInner()) 
                return false;

            IsSelected = true;
            RaiseOnSelectedChanged();

            return true;
        }

        protected abstract bool SelectInner();

        public bool Deselect()
        {
            if (!IsSelected)
                return true;

            if (!DeselectInner()) 
                return false;

            IsSelected = false;
            RaiseOnSelectedChanged();

            return true;
        }

        protected abstract bool DeselectInner();

        protected void RaiseOnSelectedChanged() => 
            onSelectedChanged?.Invoke(new SelectedChangedEventArgs(this, IsSelected));
    }

    public record SelectedChangedEventArgs(AbstractSelectable Selectable, bool IsSelected);
}
