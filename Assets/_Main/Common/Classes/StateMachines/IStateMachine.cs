using Cysharp.Threading.Tasks;

namespace Main.Common.Classes.StateMachines
{
    public interface IStateMachine
    {
        public IExitableState CurrentState { get; }

        public UniTask ChangeState<T>(IState<T> state, T enterArgs) 
            where T : IStateEnterArgs;
    }
}