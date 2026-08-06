using Main.Gameplay.Checkerboards;

namespace Main.Gameplay.Pawns
{
    public record PawnEnterCheckerboardEventArgs(IPawnDraggable Draggable, ICheckerboard Checkerboard);
}
