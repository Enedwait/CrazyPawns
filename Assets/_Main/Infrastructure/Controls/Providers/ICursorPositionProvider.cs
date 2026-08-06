using UnityEngine;

namespace Main.Infrastructure.Controls.Providers
{
    public interface ICursorPositionProvider : IInputProvider
    {
        Vector2 CursorPosition { get; }

        Ray GetCameraRay(Camera camera);
        Vector3 GetWorldPositionWithY(Camera camera, float y = 0f);
    }
}
