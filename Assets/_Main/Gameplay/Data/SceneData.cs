using UnityEngine;

namespace Main.Gameplay.Data
{
    public class SceneData : MonoBehaviour
    {
        [field: SerializeField] public Camera MainCamera { get; protected set; }
    }
}
