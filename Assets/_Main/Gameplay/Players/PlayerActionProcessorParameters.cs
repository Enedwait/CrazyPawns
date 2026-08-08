using Main.Gameplay.Managers.Connection;
using Main.Gameplay.Managers.Drag;
using Main.Gameplay.Managers.Pan;
using Main.Gameplay.Managers.Selection;
using Main.Gameplay.Managers.Zoom;

namespace Main.Gameplay.Players
{
    public record PlayerActionProcessorParameters(
        ISelectionManager SelectionManager,
        IPanManager PanManager,
        IZoomManager ZoomManager,
        IDragManager DragManager,
        IConnectionManager ConnectionManager);
}
