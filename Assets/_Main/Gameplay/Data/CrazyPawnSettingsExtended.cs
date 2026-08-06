using CrazyPawn;
using UnityEngine;

namespace Main.Gameplay.Data
{
    [CreateAssetMenu(menuName = "CrazyPawn/Data/Settings Extended SO", fileName = "New CrazyPawnSettingsExtendedSO")]
    public sealed class CrazyPawnSettingsExtendedSO : ScriptableObject
    {
        [field: SerializeField] public CrazyPawnSettings Original { get; private set; }
        [field: SerializeField] public Material SelectedMaterial { get; private set; }
        [field: SerializeField] public float CellSize { get; private set; } = 1.5f;
        [field: SerializeField] public int InitialConnectionCount { get; private set; } = 64;
        [field: SerializeField] public float ConnectionWidth { get; private set; } = 0.07f;
    }
}
