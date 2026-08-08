using Cysharp.Threading.Tasks;
using Main.Common.Classes.Objects;
using Main.Common.Classes.StateMachines;

namespace Main.Gameplay.Players.States
{
    public abstract class AbstractPlayerState<T> : AbstractSubscriber, IState<T>, IExitableState
        where T : IStateEnterArgs
    {
        #region Fields

        protected IPlayerStateController controller;
        protected IManagerHolder managerHolder;

        #endregion

        #region Init

        protected AbstractPlayerState(IManagerHolder managerHolder, IPlayerStateController controller)
        {
            this.managerHolder = managerHolder;
            this.controller = controller;
        }

        #endregion

        #region Methods

        public abstract UniTask Enter(T enterArgs);

        public virtual async UniTask Exit()
        {
            Subscribe(false);
        }

        #endregion
    }
}
