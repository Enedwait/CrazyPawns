using Main.Common.Behaviours;

namespace Main.Gameplay.Managers.Drag
{
    public record DragStartedEventArgs(IDragManager DragManager, IDraggable Draggable);
}
