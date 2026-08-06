using System.Collections.Generic;
using Main.Common.Extensions;

namespace Main.Common.Classes.Registries
{
    public class Registry<T>
    {
        #region Fields

        private HashSet<T> _items;

        #endregion

        #region Properties

        public IReadOnlyCollection<T> Items => _items;

        #endregion

        #region Init

        public Registry(int capacity)
        {
            _items = new HashSet<T>(capacity);
        }

        #endregion

        #region Register

        public void Register(T instance)
        {
            if (instance.IsNullOrDestroyed()) return;
            if (_items.Contains(instance)) return;

            _items.Add(instance);
        }

        public void Unregister(T instance)
        {
            if (instance == null) return;

            _items.Remove(instance);
        }

        #endregion
    }
}
