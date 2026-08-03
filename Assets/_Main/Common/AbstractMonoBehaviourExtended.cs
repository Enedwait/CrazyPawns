using UnityEngine;

namespace Main.Common
{
    public abstract class AbstractMonoBehaviourExtended : MonoBehaviour
    {
        #region Unity Methods

        protected virtual void Start()
        {
            Subscribe(true);
        }

        protected virtual void OnDestroy()
        {
            Subscribe(false);
        }

        #endregion

        #region Subscribe

        protected abstract void Subscribe(bool subscribe);

        #endregion
    }
}
