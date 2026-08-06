using Main.Gameplay.Targets;

namespace Main.Gameplay.Managers.PanAndZoom
{
    public interface IPanAndZoomManager : IManager
    {
        bool IsPanAllowed { get; }
        bool IsZoomAllowed { get; }

        void SetTarget(PanAndZoomTarget target);
        void SetPanAllowed(bool allowed);
        void SetZoomAllowed(bool allowed);
    }
}
