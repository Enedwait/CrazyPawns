using Main.Gameplay.Targets;
using UnityEngine;

namespace Main.Gameplay.Players
{
    public record PlayerInitArgs(Camera Camera, PanAndZoomTarget PanAndZoomTarget);
}
