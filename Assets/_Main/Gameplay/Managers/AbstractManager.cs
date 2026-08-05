using Main.Common.Behaviours;

namespace Main.Gameplay.Managers
{
    public abstract class AbstractManager : AbstractMonoBehaviourExtended
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

        #region SetActive

        public void SetActive(bool active)
        {
            IsActive = active;
        }

        #endregion
    }
}
