using UnityEngine;

namespace Main.Gameplay.Managers.Pan
{
    public interface IPanTarget
    {
        void SetPan(Vector2 panDelta);
    }
}
