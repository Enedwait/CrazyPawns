using UnityEngine;
using UnityEngine.Events;

namespace Main.Common.Behaviours
{
    public abstract class AbstractSelectable : AbstractMonoBehaviourExtended, ISelectable
    {
        #region Fields

        [SerializeField] protected Transform target;

        #endregion

        #region Properties

        public Transform Target => target;
        public bool IsSelected { get; protected set; }
        public bool CanSelect { get; protected set; }

        #endregion

        #region Events

        public event UnityAction<SelectedChangedEventArgs> onSelectedChanged;

        #endregion

        #region Unity Methods

        protected override void Start()
        {
            base.Start();

            CanSelect = true;
        }

        #endregion

        #region Init

        protected override void InitComponents()
        {
            base.InitComponents();

            if (target == null)
                target = transform;
        }

        #endregion

        #region Select

        public bool Select()
        {
            if (!CanSelect || IsSelected) return false;
            if (!SelectInner()) return false;

            IsSelected = true;
            RaiseOnSelectedChanged();

            return true;
        }

        protected abstract bool SelectInner();

        #endregion

        #region Deselect

        public bool Deselect()
        {
            if (!IsSelected) return true;
            if (!DeselectInner()) return false;

            IsSelected = false;
            RaiseOnSelectedChanged();

            return true;
        }

        protected abstract bool DeselectInner();

        #endregion

        #region Event Raisers

        protected void RaiseOnSelectedChanged() => 
            onSelectedChanged?.Invoke(new SelectedChangedEventArgs(this, IsSelected));

        #endregion
    }

    public interface ISelectable
    {
        event UnityAction<SelectedChangedEventArgs> onSelectedChanged;

        bool IsSelected { get; }
        bool Select();
        bool Deselect();
    }

    public record SelectedChangedEventArgs(AbstractSelectable Selectable, bool IsSelected);
}
