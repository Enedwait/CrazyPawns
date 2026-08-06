using Main.Common.Behaviours;

namespace Main.Gameplay.Managers.Drag
{
    public record DragEndedEventArgs(IDragManager DragManager, IDraggable Draggable);
}
