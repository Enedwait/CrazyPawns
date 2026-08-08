using Cysharp.Threading.Tasks;

namespace Main.Common.Classes.StateMachines
{
    public interface IState<T> : IExitableState 
        where T : IStateEnterArgs
    {
        UniTask Enter(T enterArgs);
    }

    public interface IExitableState
    {
        UniTask Exit();
    }
}