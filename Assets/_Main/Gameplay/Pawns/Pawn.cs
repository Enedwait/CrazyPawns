using UnityEngine;
using Zenject;

namespace Main.Gameplay.Pawns
{
    public class Pawn : MonoBehaviour, IPoolable<IMemoryPool>
    {
        private IMemoryPool _pool;

        public void OnSpawned(IMemoryPool pool)
        {
            _pool = pool;
        }

        public void OnDespawned()
        {
            _pool = null;
        }
    }
}
