using UnityEngine;
using UnityEngine.Events;
using System.Runtime.CompilerServices;

namespace Main.Common.Behaviours
{
    public abstract class AbstractSelectable : MonoBehaviour
    {
        [SerializeField] protected Transform target;
        
        public Transform Target => target;
        public bool IsSelected { get; protected set; }
        public bool CanSelect { get; protected set; }

        public event UnityAction<SelectedChangedEventArgs> onSelectedChanged;

        public bool Select()
        {
            if (!CanSelect && IsSelected)
                return false;

            if (SelectInner())
            {
                IsSelected = true;
                RaiseOnSelectedChanged();
                return true;
            }

            return false;
        }

        protected abstract bool SelectInner();

        public bool Deselect()
        {
            if (!IsSelected)
                return true;

            if (DeselectInner())
            {
                IsSelected = false;
                RaiseOnSelectedChanged();
                return true;
            }

            return false;
        }

        protected abstract bool DeselectInner();

        protected void RaiseOnSelectedChanged() => 
            onSelectedChanged?.Invoke(new SelectedChangedEventArgs(this, IsSelected));
    }

    public record SelectedChangedEventArgs(AbstractSelectable Selectable, bool IsSelected);
}
