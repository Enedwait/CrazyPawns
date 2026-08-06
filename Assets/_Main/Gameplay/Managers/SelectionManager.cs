using Main.Common.Behaviours;
using Main.Common.Extensions;
using Main.Gameplay.Cameras;
using Main.Gameplay.Players;
using Main.Infrastructure.Controls.Providers;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Main.Gameplay.Managers
{
    [DisallowMultipleComponent]
    public class SelectionManager : AbstractManager
    {
        [SerializeField] protected int _maxHitsCount = 16;
        [SerializeField] private float _maxDistance = 1000f;
        [SerializeField] private LayerMask _layersToCheck;

        private RaycastHit[] _hits;
        private ICameraProvider _cameraProvider;
        private ClickProvider _clickProvider;
        private CursorPositionProvider _cursorPositionProvider;
        private Camera Camera => _cameraProvider.GetCamera();

        public ISelectable Current { get; private set; }

        public event UnityAction<SelectedEventArgs> onSelected;
        public event UnityAction<SelectedEventArgs> onReleased;

        #region Inject

        [Inject]
        private void Construct(
            ICameraProvider cameraProvider,
            PlayerInputHandler inputHandler)
        {
            this._cameraProvider = cameraProvider;
            this._cursorPositionProvider = inputHandler.CursorPositionProvider;
            this._clickProvider = inputHandler.ClickProvider;
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

        public bool Select(AbstractSelectable selectable)
        {
            if (!IsActive)
                return false;

            if (selectable == null)
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

        public bool Deselect(ISelectable selectable)
        {
            if (!IsActive)
                return false;

            if (selectable == null)
                return true;
            
            if (!selectable.Deselect())
                return false;

            SubscribeToSelectable(selectable, false);
            RaiseOnReleased(selectable);
            return true;
        }

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
            {
                Deselect(args.Selectable);
            }
        }

        #endregion

        #region Event Raisers

        private void RaiseOnSelected(ISelectable selectable) => 
            onSelected?.Invoke(new SelectedEventArgs(this, selectable));

        private void RaiseOnReleased(ISelectable selectable) => 
            onReleased?.Invoke(new SelectedEventArgs(this, selectable));

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            if (_clickProvider == null)
                return;

            if (subscribe)
            {
                _clickProvider.onClickPerformed += OnClick;
                _clickProvider.onClickCanceled += OnClickCanceled;
            }
            else
            {
                _clickProvider.onClickPerformed -= OnClick;
                _clickProvider.onClickCanceled -= OnClickCanceled;
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

        private void OnClickCanceled()
        { }

        #endregion
    }

    public record SelectedEventArgs(SelectionManager Manager, ISelectable Selectable);
}
