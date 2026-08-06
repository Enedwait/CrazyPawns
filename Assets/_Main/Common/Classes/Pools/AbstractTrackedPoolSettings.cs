namespace Main.Common.Classes.Pools
{
    public abstract class AbstractTrackedPoolSettings : ITrackedPoolSettings
    {
        #region Properties

        public int InitialActiveItemsCapacity { get; }

        #endregion

        #region Init

        protected AbstractTrackedPoolSettings(int initialCapacity)
        {
            InitialActiveItemsCapacity = initialCapacity;
        }

        #endregion
    }

    public interface ITrackedPoolSettings
    {
        int InitialActiveItemsCapacity { get; }
    }
}
