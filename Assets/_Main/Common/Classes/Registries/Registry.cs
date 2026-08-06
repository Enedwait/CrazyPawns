using System.Collections.Generic;

namespace Main.Common.Classes.Registries
{
    public class Registry<T>
    {
        private List<T> _items;

        public IReadOnlyCollection<T> Items => _items;

        public Registry(int capacity)
        {
            _items = new List<T>(capacity);
        }

        public void Register(T instance)
        {
            if (instance == null) return;
            if (_items.Contains(instance)) return;

            _items.Add(instance);
        }

        public void Unregister(T instance)
        {
            _items.Remove(instance);
        }
    }
}
