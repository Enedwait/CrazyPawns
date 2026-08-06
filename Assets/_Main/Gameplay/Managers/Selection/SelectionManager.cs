using Main.Common.Behaviours;
using Main.Common.Extensions;
using Main.Gameplay.Players;
using Main.Infrastructure.Controls.Providers;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Main.Gameplay.Managers.Selection
{
    [DisallowMultipleComponent]
    public class SelectionManager : AbstractManager, ISelectionManager
    {
        #region Fields

        [SerializeField] protected int _maxHitsCount = 16;
        [SerializeField] private float _maxDistance = 1000f;
        [SerializeField] private LayerMask _layersToCheck;

        private RaycastHit[] _hits;
        private ICameraProvider _cameraProvider;
        private IClickProvider _clickProvider;
        private ICursorPositionProvider _cursorPositionProvider;

        #endregion

        #region Properties

        public ISelectable Current { get; private set; }
        private Camera Camera => _cameraProvider.GetCamera();

        #endregion

        #region Events

        public event UnityAction<SelectedEventArgs> onSelected;
        public event UnityAction<DeselectedEventArgs> onDeselected;

        #endregion

        #region Inject

        [Inject]
        private void Construct(
            ICameraProvider cameraProvider,
            IPlayerInputHolder inputHolder)
        {
            this._cameraProvider = cameraProvider;
            this._cursorPositionProvider = inputHolder.CursorPositionProvider;
            this._clickProvider = inputHolder.ClickProvider;
        }

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();

            _hits = new RaycastHit[_maxHitsCount];
        }

        #endregion

        #region Select

        public bool Select(ISelectable selectable)
        {
            if (!IsActive)
                return false;

            if (selectable.IsNullOrDestroyed())
                return false;

            if (!Deselect(Current))
                return false;

            if (!selectable.Select())
                return false;

            SubscribeToSelectable(selectable, true);
            Current = selectable;

            RaiseOnSelected(Current);
            return true;
        }

        #endregion

        #region Deselect

        public bool Deselect(ISelectable selectable)
        {
            if (!IsActive)
                return false;

            if (selectable == null)
                return true;

            if (!selectable.Deselect())
                return false;

            FinalizeDeselect(selectable);
            return true;
        }

        protected void FinalizeDeselect(ISelectable selectable)
        {
            SubscribeToSelectable(selectable, false);
            RaiseOnDeselected(selectable);
        }

        #endregion

        #region Selectable

        private void SubscribeToSelectable(ISelectable selectable, bool subscribe)
        {
            if (selectable == null)
                return;

            if (subscribe)
            {
                selectable.onSelectedChanged += OnSelectedChanged;
            }
            else
            {
                selectable.onSelectedChanged -= OnSelectedChanged;
            }
        }

        private void OnSelectedChanged(SelectedChangedEventArgs args)
        {
            if (args.Selectable == null)
                return;

            if (args.IsSelected)
            { }
            else
                Deselect(args.Selectable);
        }

        #endregion

        #region Event Raisers

        private void RaiseOnSelected(ISelectable selectable) => 
            onSelected?.Invoke(new SelectedEventArgs(this, selectable));

        private void RaiseOnDeselected(ISelectable selectable) => 
            onDeselected?.Invoke(new DeselectedEventArgs(this, selectable));

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            if (_clickProvider == null)
                return;

            if (subscribe)
            {
                _clickProvider.onClickPerformed += OnClick;
            }
            else
            {
                _clickProvider.onClickPerformed -= OnClick;
            }
        }

        private void OnClick()
        {
            if (!IsActive)
                return;

            Vector2 screenPosition = _cursorPositionProvider.CursorPosition;
            Ray cameraRay = Camera.ScreenPointToRay(screenPosition);

            int hitsCount = Physics.RaycastNonAlloc(cameraRay, _hits, _maxDistance, _layersToCheck);
            if (hitsCount > 0)
            {
                RaycastHit closestHit = _hits.GetClosestHit(hitsCount);

                AbstractSelectable selectable = closestHit.collider.GetComponent<AbstractSelectable>();
                if (Select(selectable))
                {
                    return;
                }
            }

            Deselect(Current);
        }

        #endregion
    }
}
