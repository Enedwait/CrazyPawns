using Main.Common.Behaviours;
using Zenject;

namespace Main.Gameplay.Pawns
{
    public sealed class PawnSelectable : AbstractSelectable, IPawnSelectable
    {
        #region Properties

        public IDraggable Draggable { get; private set; }

        #endregion

        #region Inject

        [Inject]
        private void Construct(PawnDraggable pawnDraggable)
        {
            Draggable = pawnDraggable;
        }

        #endregion

        #region Select

        protected override bool SelectInner()
        {
            return true;
        }

        protected override bool DeselectInner()
        {
            return true;
        }

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        { }

        #endregion
    }

    public interface IPawnSelectable : ISelectable
    {
        IDraggable Draggable { get; }
    }
}
