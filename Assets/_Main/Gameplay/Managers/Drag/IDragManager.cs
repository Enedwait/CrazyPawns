using Main.Common.Behaviours;
using UnityEngine.Events;

namespace Main.Gameplay.Managers.Drag
{
    public interface IDragManager : IManager
    {
        event UnityAction<DragStartedEventArgs> onDragStarted;
        event UnityAction<DragAttemptedEventArgs> onDragAttempted;
        event UnityAction<DragEndedEventArgs> onDragCompleted;

        bool BeginDrag(IDraggable draggable);
        bool EndDrag();
    }
}
