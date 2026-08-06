namespace Main.Common.Classes.Objects
{
    public abstract class AbstractSubscriber : DisposableObject
    {
        #region Fields

        protected bool isSubscribed;

        #endregion

        #region Init

        protected AbstractSubscriber()
        { }

        #endregion

        #region Subscribe

        protected void Subscribe(bool subscribe)
        {
            if (subscribe && isSubscribed)
                SubscribeInner(false);

            SubscribeInner(subscribe);
            isSubscribed = subscribe;
        }

        protected abstract void SubscribeInner(bool subscribe);

        #endregion

        #region Dispose

        protected override void DisposeManaged()
        {
            Subscribe(false);
        }

        #endregion
    }
}
