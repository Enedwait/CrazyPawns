using Main.Common.Behaviours;
using UnityEngine.Events;

namespace Main.Gameplay.Pawns
{
    public interface IPawnDraggable : IDraggable
    {
        event UnityAction<PawnEnterCheckerboardEventArgs> onEnterCheckerboard;
        event UnityAction<PawnExitCheckerboardEventArgs> onExitCheckerboard;
        event UnityAction<PawnDragEndedOutsideEventArgs> onDragEndedOutside;
    }
}
