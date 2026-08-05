using Main.Common.Classes;
using System;
using UnityEngine;

namespace Main.Gameplay.Connections
{
    public class ConnectionPool : TrackedMonoPool<Connection, ConnectionPoolSettings>
    {
        protected override void OnCreated(Connection item)
        {
            base.OnCreated(item);
            item.gameObject.SetActive(false);
        }

        protected override void OnSpawned(Connection item)
        {
            base.OnSpawned(item);
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(Connection item)
        {
            base.OnDespawned(item);
            item.gameObject.SetActive(false);
        }

        protected override void Reinitialize(Connection item)
        {
            base.Reinitialize(item);
            item.ResetValues();
            item.transform.position = Vector3.zero;
        }
    }

    [Serializable]
    public class ConnectionPoolSettings : AbstractPoolSettings
    {
        public ConnectionPoolSettings(int initialCapacity) : base(initialCapacity)
        { }
    }
}
