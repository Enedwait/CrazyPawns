using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Main.Common.Classes
{
    public class TrackedMonoPool<T, TSettings> : MonoMemoryPool<T> 
        where T : Component
        where TSettings : AbstractPoolSettings
    {
        private List<T> _activeItems;

        public IReadOnlyList<T> ActiveItems => _activeItems;

        [Inject]
        private void Init(TSettings settings)
        {
            _activeItems = new List<T>(settings.InitialActiveItemsCapacity);
        }

        protected override void OnSpawned(T item)
        {
            base.OnSpawned(item);
            _activeItems.Add(item);
        }

        protected override void OnDespawned(T item)
        {
            base.OnDespawned(item);
            _activeItems.Remove(item);
        }
    }

    public class TrackedMonoPool<TParam1, TValue, TSettings> : MonoMemoryPool<TParam1, TValue> 
        where TValue : Component
        where TSettings : AbstractPoolSettings
    {
        private List<TValue> _activeItems;

        public IReadOnlyList<TValue> ActiveItems => _activeItems;

        [Inject]
        private void Init(TSettings settings)
        {
            _activeItems = new List<TValue>(settings.InitialActiveItemsCapacity);
        }

        protected override void OnSpawned(TValue item)
        {
            base.OnSpawned(item);
            _activeItems.Add(item);
        }

        protected override void OnDespawned(TValue item)
        {
            base.OnDespawned(item);
            _activeItems.Remove(item);
        }
    }

    /*
    public class TrackedPool<T> : MemoryPool<T>
    {
        private List<T> _activeItems;

        public IReadOnlyList<T> ActiveItems => _activeItems;

        [Inject]
        private void Init(TrackedPoolSettings settings)
        {
            _activeItems = new List<T>(settings.InitialActiveItemsCapacity);
        }

        protected override void OnSpawned(T item)
        {
            base.OnSpawned(item);
            _activeItems.Add(item);
        }

        protected override void OnDespawned(T item)
        {
            base.OnDespawned(item);
            _activeItems.Remove(item);
        }
    }*/


    

    public abstract class AbstractPoolSettings
    {
        public int InitialActiveItemsCapacity { get; }

        protected AbstractPoolSettings(int initialCapacity)
        {
            InitialActiveItemsCapacity = initialCapacity;
        }
    }
}
