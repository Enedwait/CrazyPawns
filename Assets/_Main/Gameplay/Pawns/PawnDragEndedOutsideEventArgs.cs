using Main.Gameplay.Checkerboards;

namespace Main.Gameplay.Pawns
{
    public record PawnDragEndedOutsideEventArgs(IPawnDraggable Draggable, ICheckerboard Checkerboard);
}
