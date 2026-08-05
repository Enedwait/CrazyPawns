using Main.Common.Behaviours;
using Main.Gameplay.Cameras;
using Main.Gameplay.Data;
using Main.Gameplay.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Main.Gameplay.Players
{
    [DisallowMultipleComponent]
    public sealed class Player : AbstractMonoBehaviourExtended
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private SelectionManager _selectionManager;
        [SerializeField] private PanAndZoomManager _panAndZoomManager;
        [SerializeField] private DragManager _dragManager;

        private SceneData _sceneData;
        private ICameraProvider _cameraProvider;

        public PlayerActionState ActionState { get; private set; }

        #region Inject

        [Inject]
        private void Construct(SceneData sceneData, ICameraProvider cameraProvider)
        {
            this._sceneData = sceneData;
            this._cameraProvider = cameraProvider;
        }

        #endregion

        #region Unity Methods

        protected override void Start()
        {
            base.Start();

            _playerInput.camera = _cameraProvider.GetCamera();

            _selectionManager.SetActive(true);
            _panAndZoomManager.SetActive(true);
            _dragManager.SetActive(true);

            _panAndZoomManager.SetTarget(_sceneData.MainPanAndZoomTarget);
        }

        #endregion

        #region Subscribe

        protected override void Subscribe(bool subscribe)
        {
            SubscribeToSelectionManager(subscribe);
            SubscribeToDragManager(subscribe);
        }

        #endregion

        #region SelectionManager

        private void SubscribeToSelectionManager(bool subscribe)
        {
            if (_selectionManager == null)
                return;

            if (subscribe)
            {
                _selectionManager.onSelected += OnSelected;
                _selectionManager.onReleased += OnReleased;
            }
            else
            {
                _selectionManager.onSelected -= OnSelected;
                _selectionManager.onReleased -= OnReleased;
            }
        }

        private void OnSelected(AbstractSelectable selected)
        {
            //Debug.Log($"Selected: {selected}");

            _dragManager.EndDrag();

            AbstractDraggable draggable = selected.GetComponent<AbstractDraggable>();
            if (draggable != null)
            {
                //Debug.Log($"Begin Drag: {draggable}");
                _dragManager.BeginDrag(draggable);
            }
        }

        private void OnReleased(AbstractSelectable selected)
        {
            //Debug.Log($"Released: {selected}");

            //Debug.Log($"End Drag: {_dragManager.Current}");
            _dragManager.EndDrag();
        }

        #endregion

        #region DragManager

        private void SubscribeToDragManager(bool subscribe)
        {
            if (_dragManager == null)
                return;

            if (subscribe)
            {
                _dragManager.onDragStarted += OnDragStarted;
                _dragManager.onDragCompleted += OnDragCompleted;
            }
            else
            {
                _dragManager.onDragStarted -= OnDragStarted;
                _dragManager.onDragCompleted -= OnDragCompleted;
            }
        }

        private void OnDragStarted(AbstractDraggable draggable)
        {
            ActionState = PlayerActionState.Dragging;
            _panAndZoomManager.SetActive(false);
        }

        private void OnDragCompleted(AbstractDraggable draggable)
        {
            ActionState = PlayerActionState.None;
            _panAndZoomManager.SetActive(true);
        }

        #endregion
    }


    public enum PlayerActionState { None, Dragging }
}
