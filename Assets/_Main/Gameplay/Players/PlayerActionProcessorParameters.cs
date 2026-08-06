using Main.Gameplay.Managers.Connection;
using Main.Gameplay.Managers.Drag;
using Main.Gameplay.Managers.PanAndZoom;
using Main.Gameplay.Managers.Selection;

namespace Main.Gameplay.Players
{
    public record PlayerActionProcessorParameters(
        ISelectionManager SelectionManager,
        IPanAndZoomManager PanAndZoomManager,
        IDragManager DragManager,
        IConnectionManager ConnectionManager);
}
