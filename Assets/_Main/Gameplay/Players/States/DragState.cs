using Cysharp.Threading.Tasks;
using Main.Common.Behaviours;
using Main.Common.Classes.StateMachines;
using Main.Common.Extensions;
using Main.Gameplay.Managers.Drag;
using Main.Gameplay.Managers.Pan;
using Main.Gameplay.Managers.Selection;
using System;

namespace Main.Gameplay.Players.States
{
    public sealed class DragState : AbstractPlayerState<IDragStateEnterArgs>
    {
        #region Fields

        private ISelectionManager _selectionManager => managerHolder.SelectionManager;
        private IPanManager _panManager => managerHolder.PanManager;
        private IDragManager _dragManager => managerHolder.DragManager;

        #endregion

        #region Init

        public DragState(IManagerHolder managerHolder, IPlayerStateController controller) 
            : base(managerHolder, controller)
        { }

        #endregion

        #region Enter

        public override async UniTask Enter(IDragStateEnterArgs args)
        {
            if (args is DragStateEnterArgs dragArgs)
            {
                IDraggable draggable = dragArgs.Draggable;
                if (draggable.IsNullOrDestroyed())
                {
                    controller.ToIdle();
                    return;
                }

                _selectionManager.SetActive(false);
                _panManager.SetActive(false);

                _dragManager.SetActive(true);
                _dragManager.BeginDrag(draggable);

                Subscribe(true);
            }
            else
                throw new NotSupportedException($"The state enter arguments of type '{args?.GetType().Name}' are not supported in '{this.GetType().Name}'!");
        }

        #endregion

        #region Exit

        public override async UniTask Exit()
        {
            _dragManager.SetActive(false);
            _dragManager.EndDrag();

            await base.Exit();
        }

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            SubscribeToDragManager(subscribe);
        }

        #endregion

        #region Drag

        private void SubscribeToDragManager(bool subscribe)
        {
            if (_dragManager == null)
                return;

            if (subscribe) _dragManager.onDragCompleted += OnDragCompleted;
            else _dragManager.onDragCompleted -= OnDragCompleted;
        }

        private void OnDragCompleted(Managers.Drag.DragEndedEventArgs args)
        {
            _dragManager.onDragCompleted -= OnDragCompleted;
            controller.ToIdle();
        }

        #endregion
    }

    public record DragStateEnterArgs(IDraggable Draggable) : IDragStateEnterArgs;

    public interface IDragStateEnterArgs : IStateEnterArgs
    { }
}