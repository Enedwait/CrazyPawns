using Main.Common.Behaviours;
using Main.Common.Extensions;
using Main.Gameplay.Cameras;
using Main.Infrastructure.Controls.Providers;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace Main.Gameplay.Managers
{
    [DisallowMultipleComponent]
    public class SelectionManager : AbstractMonoBehaviourExtended
    {
        [SerializeField] private ClickProvider _clickProvider;
        [SerializeField] private CursorPositionProvider _cursorPositionProvider;
        [SerializeField] protected int _maxHitsCount = 16;
        [SerializeField] private float _maxDistance = 1000f;
        [SerializeField] private LayerMask _layersToCheck;

        private RaycastHit[] _hits;
        private ICameraProvider _cameraProvider;
        private Camera Camera => _cameraProvider.GetCamera();

        public bool IsActive { get; private set; }
        public AbstractSelectable Current { get; private set; }

        public event UnityAction<AbstractSelectable> onSelected;
        public event UnityAction<AbstractSelectable> onReleased;

        #region Inject

        [Inject]
        private void Construct(ICameraProvider cameraProvider)
        {
            this._cameraProvider = cameraProvider;
        }

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _hits = new RaycastHit[_maxHitsCount];
        }

        protected override void OnDestroy()
        {
            SetActive(false);
            base.OnDestroy();
        }

        #endregion

        #region SetActive

        public void SetActive(bool active)
        {
            IsActive = active;
        }

        #endregion

        #region Select

        public bool Select(AbstractSelectable selectable)
        {
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

        public bool Deselect(AbstractSelectable selectable)
        {
            if (selectable == null)
                return true;
            
            if (!selectable.Deselect())
                return false;

            SubscribeToSelectable(selectable, false);
            RaiseOnReleased(selectable);
            return true;
        }

        private void SubscribeToSelectable(AbstractSelectable selectable, bool subscribe)
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
                Select(selectable);
            }
        }

        private void OnClickCanceled()
        { }

        private void RaiseOnSelected(AbstractSelectable selectable) => onSelected?.Invoke(selectable);
        private void RaiseOnReleased(AbstractSelectable selectable) => onReleased?.Invoke(selectable);

        #region Subscribe

        protected override void Subscribe(bool subscribe)
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

        #endregion

        
    }
}
