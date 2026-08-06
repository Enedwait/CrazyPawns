using UnityEngine.Events;

namespace Main.Infrastructure.Controls.Providers
{
    public interface IClickProvider : IInputProvider
    {
        event UnityAction onClickStarted;
        event UnityAction onClickPerformed;
        event UnityAction onClickCanceled;
    }
}
