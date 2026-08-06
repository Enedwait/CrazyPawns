using Cysharp.Threading.Tasks;

namespace Main.Gameplay.Players
{
    public interface IPlayerActionProcessor
    {
        UniTask InitializeAsync(PlayerActionProcessorInitArgs args);
    }
}
