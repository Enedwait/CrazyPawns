using Zenject;

namespace Main.Gameplay.Players
{
    public sealed class PlayerFactory : PlaceholderFactory<IPlayer>, IPlayerFactory
    { }

    public interface IPlayerFactory : IFactory<IPlayer>
    { }
}
