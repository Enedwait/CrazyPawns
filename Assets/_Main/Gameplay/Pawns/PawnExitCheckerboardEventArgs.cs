using Main.Gameplay.Checkerboards;

namespace Main.Gameplay.Pawns
{
    public record PawnExitCheckerboardEventArgs(IPawnDraggable Draggable, ICheckerboard Checkerboard);
}
