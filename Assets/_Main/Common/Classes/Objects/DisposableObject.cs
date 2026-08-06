using System;

namespace Main.Common.Classes.Objects
{
    public abstract class DisposableObject : IDisposable
    {
        #region Fields

        protected bool isDisposed = false;

        #endregion

        #region Dispose

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
