using Cysharp.Threading.Tasks;
using Main.Common.Classes.StateMachines;
using Main.Gameplay.Managers.Pan;
using Main.Gameplay.Managers.Selection;
using Main.Gameplay.Managers.Zoom;

namespace Main.Gameplay.Players.States
{
    public sealed class IdleState : AbstractPlayerState<IStateEnterArgs>
    {
        #region Fields

        private ISelectionManager _selectionManager => managerHolder.SelectionManager;
        private IPanManager _panManager => managerHolder.PanManager;
        private IZoomManager _zoomManager => managerHolder.ZoomManager;

        #endregion

        #region Init

        public IdleState(IManagerHolder managerHolder, IPlayerStateController controller) 
            : base(managerHolder, controller)
        { }

        #endregion

        #region Enter

        public override async UniTask Enter(IStateEnterArgs args)
        {
            managerHolder.DeactivateAll();

            _selectionManager.SetActive(true);

            _panManager.SetActive(true);
            _zoomManager.SetActive(true);

            Subscribe(true);
        }

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            SubscribeToSelectionManager(subscribe);
        }

        #endregion

        #region SelectionManager

        private void SubscribeToSelectionManager(bool subscribe)
        {
            if (_selectionManager == null)
                return;

            if (subscribe) _selectionManager.onSelected += OnSelected;
            else _selectionManager.onSelected -= OnSelected;
        }

        private void OnSelected(Managers.Selection.SelectedEventArgs args)
        {
            _selectionManager.onSelected -= OnSelected;
            controller.ToSelection(new SelectionStateEnterArgs(args.Selectable))
                .Forget();
        }
        
        #endregion
    }
}
