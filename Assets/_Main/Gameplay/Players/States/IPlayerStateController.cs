using Cysharp.Threading.Tasks;

namespace Main.Gameplay.Players.States
{
    public interface IPlayerStateController
    {
        UniTask ToIdle();
        UniTask ToSelection(ISelectionStateEnterArgs args);
        UniTask ToDrag(IDragStateEnterArgs args);
        UniTask ToConnection(IConnectionStateEnterArgs args);
    }
}
