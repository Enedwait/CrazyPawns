using Cysharp.Threading.Tasks;

namespace Main.Gameplay.Players
{
    public interface IPlayer
    {
        UniTask InitializeAsync(PlayerInitArgs args);
    }
}
