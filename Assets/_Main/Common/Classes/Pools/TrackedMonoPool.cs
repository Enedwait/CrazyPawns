using System.Collections.Generic;
using Main.Common.Interfaces;
using UnityEngine;
using Zenject;

namespace Main.Common.Classes.Pools
{
    public class TrackedMonoPool<TComponent, TSettings> : MonoMemoryPool<TComponent>, IActiveItems<TComponent> 
        where TComponent : Component
        where TSettings : ITrackedPoolSettings
    {
        #region Fields

        private List<TComponent> _activeItems;

        #endregion

        #region Properties

        public IReadOnlyList<TComponent> ActiveItems => _activeItems;

        #endregion

        #region Init

        [Inject]
        private void Init(TSettings settings)
        {
            _activeItems = new List<TComponent>(settings.InitialActiveItemsCapacity);
        }

        #endregion

        #region Spawn

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

        #endregion
    }

    public class TrackedMonoPool<TParam1, TComponent, TSettings> : MonoMemoryPool<TParam1, TComponent>, IActiveItems<TComponent>
        where TComponent : Component
        where TSettings : ITrackedPoolSettings
    {
        #region Fields

        private List<TComponent> _activeItems;

        #endregion

        #region Properties

        public IReadOnlyList<TComponent> ActiveItems => _activeItems;

        #endregion

        #region Init

        [Inject]
        private void Init(TSettings settings)
        {
            _activeItems = new List<TComponent>(settings.InitialActiveItemsCapacity);
        }

        #endregion

        #region Spawn

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

        #endregion
    }
}
