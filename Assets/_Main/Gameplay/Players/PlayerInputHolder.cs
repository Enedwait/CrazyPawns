using Main.Infrastructure.Controls.Providers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.Gameplay.Players
{
    public sealed class PlayerInputHolder : MonoBehaviour, IPlayerInputHolder
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private ClickProvider _clickProvider;
        [SerializeField] private CursorPositionProvider _cursorPositionProvider;
        [SerializeField] private Vector2DeltaProvider _cursorDeltaProvider;
        [SerializeField] private Vector2DeltaProvider _panProvider;
        [SerializeField] private FloatDeltaProvider _zoomProvider;

        public PlayerInput PlayerInput => _playerInput;
        public IClickProvider ClickProvider => _clickProvider;
        public ICursorPositionProvider CursorPositionProvider => _cursorPositionProvider;
        public IVector2DeltaProvider CursorDeltaProvider => _cursorDeltaProvider;
        public IVector2DeltaProvider PanProvider => _panProvider;
        public IFloatDeltaProvider ZoomProvider => _zoomProvider;
    }
}
