using Main.Common.Behaviours;

namespace Main.Gameplay.Animations
{
    public abstract class AbstractEntityAnimator : AbstractMonoBehaviourExtended
    {
        protected abstract void PlayIdle();
        protected abstract void PlayActive();
        protected abstract void PlayDelete();
    }
}
