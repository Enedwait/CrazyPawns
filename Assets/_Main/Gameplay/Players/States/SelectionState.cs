using Cysharp.Threading.Tasks;
using Main.Common.Behaviours;
using Main.Common.Classes.StateMachines;
using Main.Common.Extensions;
using Main.Gameplay.Connectors;
using Main.Gameplay.Managers.Drag;
using Main.Gameplay.Managers.Pan;
using Main.Gameplay.Managers.Selection;
using Main.Gameplay.Pawns;
using System;

namespace Main.Gameplay.Players.States
{
    public sealed class SelectionState : AbstractPlayerState<ISelectionStateEnterArgs>
    {
        #region Fields

        private IDraggable _draggable;
        private ISelectionManager _selectionManager => managerHolder.SelectionManager;
        private IPanManager _panManager => managerHolder.PanManager;
        private IDragManager _dragManager => managerHolder.DragManager;

        #endregion

        #region Init

        public SelectionState(IManagerHolder managerHolder, IPlayerStateController controller) 
            : base(managerHolder, controller)
        { }

        #endregion

        #region Enter

        public override async UniTask Enter(ISelectionStateEnterArgs args)
        {
            if (args is SelectionStateEnterArgs selectionArgs)
            {
                ISelectable selectable = selectionArgs.Selectable;
                if (selectable.IsNullOrDestroyed())
                {
                    controller.ToIdle();
                    return;
                }

                _panManager.SetActive(false);
                ProcessSelectable(selectionArgs.Selectable);
            }
            else
                throw new NotSupportedException($"The state enter arguments of type '{args?.GetType().Name}' are not supported in '{this.GetType().Name}'!");
        }

        private void ProcessSelectable(ISelectable selectable)
        {
            switch (selectable)
            {
                case IConnectorSelectable connector: ProcessConnector(connector); break;
                case IPawnSelectable pawn: ProcessPawn(pawn); break;
                default: throw new NotImplementedException($"Unknown type passed: {selectable.GetType().FullName}");
            }
        }

        private void ProcessConnector(IConnectorSelectable selectable)
        {
            if (!selectable.IsNullOrDestroyed())
            {
                controller.ToConnection(new ConnectionStateEnterArgs(selectable));
                return;
            }

            controller.ToIdle();
        }

        private void ProcessPawn(IPawnSelectable selectable)
        {
            SubscribeToSelectionManager(true);
            _dragManager.EndDrag();

            _draggable = selectable.Draggable;
            if (!_draggable.IsNullOrDestroyed())
            {
                _dragManager.SetActive(true);
                SubscribeToDragManager(true);
                return;
            }

            // какие-то другие потенциальные варианты действий для пешки, у которой нет драга
        }

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            SubscribeToSelectionManager(subscribe);
            SubscribeToDragManager(subscribe);
        }

        #endregion

        #region Selection

        private void SubscribeToSelectionManager(bool subscribe)
        {
            if (_selectionManager == null)
                return;

            if (subscribe) _selectionManager.onDeselected += OnDeselected;
            else _selectionManager.onDeselected -= OnDeselected;
        }

        private void OnDeselected(DeselectedEventArgs arg0)
        {
            controller.ToIdle();
        }

        #endregion

        #region Drag

        private void SubscribeToDragManager(bool subscribe)
        {
            if (_selectionManager == null)
                return;

            if (subscribe) _dragManager.onDragAttempted += OnDrag;
            else _dragManager.onDragAttempted -= OnDrag;
        }

        private void OnDrag(DragAttemptedEventArgs args)
        {
            _dragManager.onDragAttempted -= OnDrag;
            controller.ToDrag(new DragStateEnterArgs(_draggable));
        }

        #endregion
    }

    public record SelectionStateEnterArgs(ISelectable Selectable) : ISelectionStateEnterArgs;

    public interface ISelectionStateEnterArgs : IStateEnterArgs
    { }
}
