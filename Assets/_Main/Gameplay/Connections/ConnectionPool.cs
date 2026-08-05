using UnityEngine;
using Zenject;

namespace Main.Gameplay.Connections
{
    public class ConnectionPool : MonoMemoryPool<Connection>
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
}
