namespace Main.Common.Classes.Pools
{
    public abstract class AbstractTrackedPoolSettings : ITrackedPoolSettings
    {
        public int InitialActiveItemsCapacity { get; }

        protected AbstractTrackedPoolSettings(int initialCapacity)
        {
            InitialActiveItemsCapacity = initialCapacity;
        }
    }

    public interface ITrackedPoolSettings
    {
        int InitialActiveItemsCapacity { get; }
    }
}
