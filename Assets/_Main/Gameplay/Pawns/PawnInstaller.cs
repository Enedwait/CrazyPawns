using Main.Common.Behaviours;
using UnityEngine;
using Zenject;

namespace Main.Gameplay.Pawns
{
    public class PawnInstaller : MonoInstaller
    {
        [SerializeField] private PawnDraggable _pawnDraggable;
        [SerializeField] private PawnSelectable _pawnSelectable;

        public override void InstallBindings()
        {
            Container.BindInstance(_pawnDraggable)
                .AsSingle();

            Container.BindInstance(_pawnSelectable)
                .AsSingle();
        }
    }
}
