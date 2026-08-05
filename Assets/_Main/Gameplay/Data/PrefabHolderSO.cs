using Main.Gameplay.Connections;
using Main.Gameplay.Pawns;
using Main.Gameplay.Players;
using UnityEngine;

namespace Main.Gameplay.Data
{
    [CreateAssetMenu(menuName = "CrazyPawn/Data/Prefab Holder SO", fileName = "New PrefabHolderSO")]
    public sealed class PrefabHolderSO : ScriptableObject
    {
        [field: SerializeField] public Player Player { get; private set; }
        [field: SerializeField] public Pawn Pawn { get; private set; }
        [field: SerializeField] public Connection Connection { get; private set; }
    }
}
