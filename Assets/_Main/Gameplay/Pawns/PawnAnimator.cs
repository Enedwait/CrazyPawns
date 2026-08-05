using Main.Gameplay.Pawns.Animations;

namespace Main.Gameplay.Animations
{
    public class PawnAnimator : AbstractPawnEntityAnimator
    {
        protected override void PlayActive()
        {
            State = PawnAnimatorState.Active;
        }
    }
}
