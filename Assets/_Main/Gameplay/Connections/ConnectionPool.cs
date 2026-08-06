using System;
using Main.Common.Classes.Pools;
using UnityEngine;

namespace Main.Gameplay.Connections
{
    public class ConnectionPool : TrackedMonoPool<Connection, ConnectionPoolSettings>, IActiveConnectionItems
    {
        protected override void OnCreated(Connection item)
        {
            base.OnCreated(item);
            if (item == null) return;
            item.gameObject.SetActive(false);
        }

        protected override void OnSpawned(Connection item)
        {
            base.OnSpawned(item);
            if (item == null) return;
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(Connection item)
        {
            base.OnDespawned(item);
            if (item == null) return;
            item.gameObject.SetActive(false);
        }

        protected override void Reinitialize(Connection item)
        {
            base.Reinitialize(item);
            if (item == null) return;
            item.ResetValues();
            item.transform.position = Vector3.zero;
        }
    }

    [Serializable]
    public class ConnectionPoolSettings : AbstractTrackedPoolSettings
    {
        public ConnectionPoolSettings(int initialCapacity) : base(initialCapacity)
        { }
    }

    public interface IActiveConnectionItems : IActiveItems<Connection>
    { }
}
