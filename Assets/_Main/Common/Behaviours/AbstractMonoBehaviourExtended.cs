using Main.Common.Extensions;
using UnityEngine;

namespace Main.Common.Behaviours
{
    public abstract class AbstractMonoBehaviourExtended : MonoBehaviour
    {
        #region Unity Methods

#if UNITY_EDITOR
        protected virtual void OnValidate()
        { }
#endif

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
