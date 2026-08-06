using Main.Gameplay.Animations;

namespace Main.Gameplay.Pawns.Animations
{
    public abstract class AbstractPawnAnimator : AbstractEntityAnimator
    {
        #region Play

        protected abstract void PlayActive();

        protected abstract void PlayDelete();

        protected abstract void PlayRestore();

        #endregion
    }
}
