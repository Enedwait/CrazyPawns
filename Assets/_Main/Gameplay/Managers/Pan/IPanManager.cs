namespace Main.Gameplay.Managers.Pan
{
    public interface IPanManager : IManager
    {
        void SetTarget(IPanTarget target);
    }
}
