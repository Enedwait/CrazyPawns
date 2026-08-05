using UnityEngine;

namespace Main.Gameplay.Connectors
{
    [DisallowMultipleComponent]
    public class ConnectorSocket : MonoBehaviour
    {
        [SerializeField] private Transform root;

        public Transform Root => root;

        private void Awake()
        {
            if (root == null)
                root = transform;
        }
    }
}
