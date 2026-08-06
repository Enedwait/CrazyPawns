using Main.Common.Behaviours;

namespace Main.Gameplay.Pawns
{
    public interface IPawnSelectable : ISelectable
    {
        IDraggable Draggable { get; }
    }
}
