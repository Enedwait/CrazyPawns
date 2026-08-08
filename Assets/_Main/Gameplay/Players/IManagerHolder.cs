using Main.Gameplay.Managers.Connection;
using Main.Gameplay.Managers.Drag;
using Main.Gameplay.Managers.Pan;
using Main.Gameplay.Managers.Selection;
using Main.Gameplay.Managers.Zoom;

namespace Main.Gameplay.Players
{
    public interface IManagerHolder
    {
        public IPanManager PanManager { get; }
        public IZoomManager ZoomManager { get; }
        public ISelectionManager SelectionManager { get; }
        public IDragManager DragManager { get; }
        public IConnectionManager ConnectionManager { get; }

        public void DeactivateAll();
    }
}
