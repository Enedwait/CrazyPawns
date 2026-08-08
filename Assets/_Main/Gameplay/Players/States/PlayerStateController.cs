using Cysharp.Threading.Tasks;
using Main.Common.Classes.StateMachines;

namespace Main.Gameplay.Players.States
{
    public class PlayerStateController : IPlayerStateController
    {
        #region Fields

        private IStateMachine _stateMachine;
        private IState<IStateEnterArgs> _idleState;
        private IState<ISelectionStateEnterArgs> _selectionState;
        private IState<IDragStateEnterArgs> _dragState;
        private IState<IConnectionStateEnterArgs> _connectionState;

        #endregion

        #region Init

        public PlayerStateController(Player player, IStateMachine stateMachine)
        {
            this._stateMachine = stateMachine;

            _idleState = new IdleState(player.ManagerHolder, this);
            _selectionState = new SelectionState(player.ManagerHolder, this);
            _dragState = new DragState(player.ManagerHolder, this);
            _connectionState = new ConnectionState(player.ManagerHolder, this);
        }

        #endregion

        #region Methods

        public async UniTask ToIdle() =>
            await _stateMachine.ChangeState(_idleState, null);

        public async UniTask ToSelection(ISelectionStateEnterArgs args) =>
            await _stateMachine.ChangeState(_selectionState, args);

        public async UniTask ToDrag(IDragStateEnterArgs args) =>
            await _stateMachine.ChangeState(_dragState, args);

        public async UniTask ToConnection(IConnectionStateEnterArgs args) =>
            await _stateMachine.ChangeState(_connectionState, args);

        #endregion
    }

    
}
