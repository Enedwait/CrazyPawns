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
            Container.Bind<IPawnDraggable>()
                .FromInstance(_pawnDraggable)
                .AsSingle();

            Container.Bind<IPawnSelectable>()
                .FromInstance(_pawnSelectable)
                .AsSingle();
        }

        #endregion
    }
}
