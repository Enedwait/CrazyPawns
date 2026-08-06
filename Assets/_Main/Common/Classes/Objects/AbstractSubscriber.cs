namespace Main.Common.Classes.Objects
{
    public abstract class AbstractSubscriber : DisposableObject
    {
        public bool IsSubscribed { get; protected set; }

        protected AbstractSubscriber()
        { }

        protected void Subscribe(bool subscribe)
        {
            if (subscribe && IsSubscribed)
                SubscribeInner(false);

            SubscribeInner(subscribe);
            IsSubscribed = subscribe;
        }

        protected abstract void SubscribeInner(bool subscribe);

        protected override void DisposeManaged()
        {
            Subscribe(false);
        }
    }
}
