using Main.Gameplay.Animations;

namespace Main.Gameplay.Connectors.Animations
{
    public abstract class AbstractConnectorAnimator : AbstractEntityAnimator
    {
        #region Play

        protected abstract void PlayActive();
        protected abstract void PlayReadyToConnect();
        protected abstract void PlayDelete();
        protected abstract void PlayRestore();

        #endregion
    }
}
