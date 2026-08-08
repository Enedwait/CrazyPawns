using Main.Gameplay.Managers.Pan;
using Main.Gameplay.Managers.Zoom;

namespace Main.Gameplay.Players
{
    public record PlayerActionProcessorInitArgs(IPanTarget PanTarget, IZoomTarget ZoomTarget);
}
