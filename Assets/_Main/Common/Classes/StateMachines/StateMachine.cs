using Cysharp.Threading.Tasks;

namespace Main.Common.Classes.StateMachines
{
    public sealed class StateMachine : IStateMachine
    {
        public IExitableState CurrentState { get; private set; }

        public async UniTask ChangeState<T>(IState<T> state, T enterArgs)
            where T : IStateEnterArgs
        {
            if (CurrentState != null)
            {
                await CurrentState.Exit();
                CurrentState = null;
            }

            if (state != null)
            {
                await state.Enter(enterArgs);
                CurrentState = state;
            }
        }
    }
}
