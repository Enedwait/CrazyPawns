using UnityEngine;

namespace Main.Common.Behaviours
{
    public abstract class AbstractMonoBehaviourExtended : MonoBehaviour
    {
        #region Fields

        protected bool isSubscribed;

        #endregion

        #region Unity Methods

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            InitComponentsOnValidate();
        }
#endif
        protected virtual void Awake()
        {
            InitComponents();
        }

        protected virtual void Start()
        {
            Subscribe(true);
        }

        protected virtual void OnDestroy()
        {
            Subscribe(false);
        }

        #endregion

        #region Init

        protected virtual void InitComponents()
        { }

        protected virtual void InitComponentsOnValidate()
        { }

        #endregion

        #region Subscribe

        protected void Subscribe(bool subscribe)
        {
            if (isSubscribed && subscribe)
                SubscribeInner(false);

            SubscribeInner(subscribe);
            isSubscribed = subscribe;
        }

        protected abstract void SubscribeInner(bool subscribe);

        #endregion
    }
}
