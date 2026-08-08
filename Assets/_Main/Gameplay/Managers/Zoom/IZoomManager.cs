namespace Main.Gameplay.Managers.Zoom
{
    public interface IZoomManager : IManager
    {
        void SetTarget(IZoomTarget target);
    }
}
