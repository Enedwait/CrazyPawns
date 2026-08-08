using UnityEngine;

namespace Main.Gameplay.Managers.Zoom
{
    public interface IZoomTarget
    {
        void SetZoom(Vector3 zoomDirection, float zoomDelta);
    }
}
