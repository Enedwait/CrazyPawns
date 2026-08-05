using Main.Common.Behaviours;
using Zenject;

namespace Main.Gameplay.Pawns
{
    public sealed class PawnSelectable : AbstractSelectable
    {
        public PawnDraggable PawnDraggable { get; private set; }

        [Inject]
        private void Construct(PawnDraggable pawnDraggable)
        {
            PawnDraggable = pawnDraggable;
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
}
