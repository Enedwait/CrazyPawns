using Main.Infrastructure.Controls.Providers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.Gameplay.Players
{
    public sealed class PlayerInputHandler : MonoBehaviour
    {
        [field: SerializeField] public PlayerInput PlayerInput { get; private set; }
        [field: SerializeField] public ClickProvider ClickProvider { get; private set; }
        [field: SerializeField] public CursorPositionProvider CursorPositionProvider { get; private set; }
        [field: SerializeField] public Vector2DeltaProvider CursorDeltaProvider { get; private set; }
        [field: SerializeField] public Vector2DeltaProvider PanProvider { get; private set; }
        [field: SerializeField] public FloatDeltaProvider ZoomProvider { get; private set; }
    }
}
