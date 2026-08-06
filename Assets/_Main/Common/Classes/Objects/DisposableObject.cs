using System;

namespace Main.Common.Classes.Objects
{
    public abstract class DisposableObject : IDisposable
    {
        #region Dispose

        protected bool isDisposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (isDisposed) return;
            if (disposing) DisposeManaged();
            DisposeUnmanaged();
            isDisposed = true;
        }

        protected abstract void DisposeManaged();

        protected virtual void DisposeUnmanaged()
        { }

        #endregion
    }
}
