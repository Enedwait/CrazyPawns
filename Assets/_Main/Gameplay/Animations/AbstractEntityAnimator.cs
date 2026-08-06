using Main.Common.Behaviours;

namespace Main.Gameplay.Animations
{
    public abstract class AbstractEntityAnimator : AbstractMonoBehaviourExtended
    {
        #region Methods

        protected abstract void PlayIdle();

        #endregion
    }
}
