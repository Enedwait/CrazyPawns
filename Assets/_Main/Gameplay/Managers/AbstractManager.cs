using Main.Common.Behaviours;

namespace Main.Gameplay.Managers
{
    public abstract class AbstractManager : AbstractMonoBehaviourExtended, IManager
    {
        #region Properties

        public bool IsActive { get; protected set; }

        #endregion

        #region Unity Methods

        protected override void OnDestroy()
        {
            SetActive(false);
            base.OnDestroy();
        }

        #endregion

        #region Methods

        public void SetActive(bool active)
        {
            IsActive = active;
        }

        #endregion
    }

    public interface IManager
    {
        bool IsActive { get; }

        void SetActive(bool active);
    }
}
