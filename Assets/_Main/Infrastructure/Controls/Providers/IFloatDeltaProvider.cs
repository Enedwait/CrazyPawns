using UnityEngine.Events;

namespace Main.Infrastructure.Controls.Providers
{
    public interface IFloatDeltaProvider : IInputProvider
    {
        event UnityAction<float> onDelta;

        float Delta { get; }
    }
}
