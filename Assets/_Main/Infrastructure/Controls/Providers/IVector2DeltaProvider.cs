using UnityEngine;
using UnityEngine.Events;

namespace Main.Infrastructure.Controls.Providers
{
    public interface IVector2DeltaProvider : IInputProvider
    {
        event UnityAction<Vector2> onDelta;
    }
}
