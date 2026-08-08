using Main.Gameplay.Managers.Connection;
using Main.Gameplay.Managers.Drag;
using Main.Gameplay.Managers.Pan;
using Main.Gameplay.Managers.Selection;
using Main.Gameplay.Managers.Zoom;

namespace Main.Gameplay.Players
{
    public record ManagerHolder(
        IPanManager PanManager,
        IZoomManager ZoomManager,
        ISelectionManager SelectionManager,
        IDragManager DragManager,
        IConnectionManager ConnectionManager) : IManagerHolder
    {
        #region DeactivateAll()

        public void DeactivateAll()
        {
            PanManager.SetActive(false);
            ZoomManager.SetActive(false);
            SelectionManager.SetActive(false);
            DragManager.SetActive(false);
            ConnectionManager.SetActive(false);
        }

        #endregion
    }
}
