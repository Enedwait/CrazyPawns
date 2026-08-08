using Main.Gameplay.Managers.Pan;
using Main.Gameplay.Managers.Zoom;
using UnityEngine;

namespace Main.Gameplay.Players
{
    public record PlayerInitArgs(Camera Camera, IPanTarget PanTarget, IZoomTarget ZoomTarget);
}
