using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Main.Common.Classes.Pools
{
    public class TrackedMonoPool<TComponent, TSettings> : MonoMemoryPool<TComponent>, IActiveItems<TComponent> 
        where TComponent : Component
        where TSettings : ITrackedPoolSettings
    {
        private List<TComponent> _activeItems;

        public IReadOnlyList<TComponent> ActiveItems => _activeItems;

        [Inject]
        private void Init(TSettings settings)
        {
            _activeItems = new List<TComponent>(settings.InitialActiveItemsCapacity);
        }

        protected override void OnSpawned(TComponent item)
        {
            if (item == null) return;
            base.OnSpawned(item);
            _activeItems.Add(item);
        }

        protected override void OnDespawned(TComponent item)
        {
            if (item == null) return;
            base.OnDespawned(item);
            _activeItems.Remove(item);
        }
    }

    public class TrackedMonoPool<TParam1, TComponent, TSettings> : MonoMemoryPool<TParam1, TComponent>, IActiveItems<TComponent>
        where TComponent : Component
        where TSettings : ITrackedPoolSettings
    {
        private List<TComponent> _activeItems;

        public IReadOnlyList<TComponent> ActiveItems => _activeItems;

        [Inject]
        private void Init(TSettings settings)
        {
            _activeItems = new List<TComponent>(settings.InitialActiveItemsCapacity);
        }

        protected override void OnSpawned(TComponent item)
        {
            if (item == null) return;
            base.OnSpawned(item);
            _activeItems.Add(item);
        }

        protected override void OnDespawned(TComponent item)
        {
            if (item == null) return;
            base.OnDespawned(item);
            _activeItems.Remove(item);
        }
    }
}
