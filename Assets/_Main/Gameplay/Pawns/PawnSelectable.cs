using Main.Common.Behaviours;

namespace Main.Gameplay.Pawns
{
    public class PawnSelectable : AbstractSelectable
    {
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
