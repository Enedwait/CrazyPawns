using Main.Common.Behaviours;
using Zenject;

namespace Main.Gameplay.Pawns
{
    public sealed class PawnSelectable : AbstractSelectable, IPawnSelectable
    {
        public IDraggable Draggable { get; private set; }

        [Inject]
        private void Construct(PawnDraggable pawnDraggable)
        {
            Draggable = pawnDraggable;
        }

        protected override bool SelectInner()
        {
            return true;
        }

        protected override bool DeselectInner()
        {
            return true;
        }

        protected override void SubscribeInner(bool subscribe)
        { }
    }

    public interface IPawnSelectable : ISelectable
    {
        IDraggable Draggable { get; }
    }
}
