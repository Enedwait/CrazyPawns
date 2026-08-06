using UnityEngine;
using Zenject;

namespace Main.Gameplay.Pawns
{
    public class PawnInstaller : MonoInstaller
    {
        #region Fields

        [SerializeField] private PawnDraggable _pawnDraggable;
        [SerializeField] private PawnSelectable _pawnSelectable;

        #endregion

        #region Install

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PawnDraggable>()
                .FromInstance(_pawnDraggable)
                .AsSingle();

            Container.BindInterfacesAndSelfTo<PawnSelectable>()
                .FromInstance(_pawnSelectable)
                .AsSingle();
        }

        #endregion
    }
}
