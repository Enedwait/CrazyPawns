using System;
using Main.Common.Behaviours;
using Main.Gameplay.Connectors.Animations;
using UnityEngine;
using Zenject;

namespace Main.Gameplay.Connectors
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(ConnectorSocket))]
    [RequireComponent(typeof(PawnConnectorAnimator))]
    [RequireComponent(typeof(ConnectorSelectable))]
    public sealed class Connector : AbstractMonoBehaviourExtended
    {
        #region Fields

        [SerializeField] private Transform root;

        private IConnectorRegistry _registry;

        #endregion

        #region Properties

        public Transform Root => root;
        public ConnectorSocket Socket { get; private set; }
        public PawnConnectorAnimator Animator { get; private set; }
        public ConnectorSelectable Selectable { get; private set; }

        #endregion

        #region Inject

        [Inject]
        private void Construct(IConnectorRegistry registry)
        {
            this._registry = registry;
        }

        #endregion

        #region Unity Methods

        private void OnEnable()
        {
            _registry.Register(this);
        }

        private void OnDisable()
        {
            _registry.Unregister(this);
        }

        protected override void OnDestroy()
        {
            _registry.Unregister(this);
            base.OnDestroy();
        }

        #endregion

        #region Init

        protected override void InitComponents()
        {
            if (root == null) 
                throw new ArgumentNullException($"The root of the {this} should be set!");

            if (Socket == null) Socket = GetComponent<ConnectorSocket>();
            if (Animator == null) Animator = GetComponent<PawnConnectorAnimator>();
            if (Selectable == null) Selectable = GetComponent<ConnectorSelectable>();
        }

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        { }

        #endregion
    }
}
