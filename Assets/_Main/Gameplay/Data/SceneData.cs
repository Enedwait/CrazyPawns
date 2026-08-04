using CrazyPawn;
using Main.Gameplay.Checkerboards;
using UnityEngine;

namespace Main.Gameplay.Data
{
    public class SceneData : MonoBehaviour
    {
        [field: SerializeField, Header("Scene Objects")] public Camera MainCamera { get; protected set; }
        [field: SerializeField] public Checkerboard Checkerboard { get; protected set; }
        [field: SerializeField] public Transform PawnSpawnRoot { get; protected set; }
        [field: SerializeField, Header("Data")] public CrazyPawnSettings CrazyPawnSettings { get; protected set; }
    }
}
