using Main.Infrastructure.Controls.Providers;
using UnityEngine.InputSystem;

namespace Main.Gameplay.Players
{
    public interface IPlayerInputHolder
    {
        public PlayerInput PlayerInput { get; }
        public IClickProvider ClickProvider { get; }
        public ICursorPositionProvider CursorPositionProvider { get; }
        public IVector2DeltaProvider CursorDeltaProvider { get; }
        public IVector2DeltaProvider PanProvider { get; }
        public IFloatDeltaProvider ZoomProvider { get; }
    }
}
