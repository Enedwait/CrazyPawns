using Main.Common.Behaviours;
using Main.Gameplay.Animations;
using Main.Gameplay.Data;
using System;
using UnityEngine;
using Zenject;

namespace Main.Gameplay.Connectors
{
    [DisallowMultipleComponent]
    public sealed class PawnConnectorAnimator : AbstractConnectorAnimator
    {
        public enum ConnectorAnimatorState { Idle, Active, Delete, Restore, ReadyToConnect }

        #region Variables

        [SerializeField] private ConnectorSelectable _connectorSelectable;
        [SerializeField] private Renderer _renderer;

        private SceneData _sceneData;
        private PawnDraggable _pawnDraggable;
        private Material _originalMaterial;

        private Material ActiveMaterial => _sceneData.CrazyPawnSettings.ActiveConnectorMaterial;
        private Material SelectedMaterial => _sceneData.Settings.SelectedMaterial;
        private Material DeleteMaterial => _sceneData.CrazyPawnSettings.DeleteMaterial;

        public ConnectorAnimatorState State { get; protected set; }

        #endregion

        #region Inject

        [Inject]
        private void Construct(SceneData sceneData, PawnDraggable pawnDraggable)
        {
            this._sceneData = sceneData;
            this._pawnDraggable = pawnDraggable;
        }

        #endregion

        #region Unity Methods

        protected override void Awake()
        {
            base.Awake();
            _originalMaterial = _renderer.material;
        }

        #endregion

        #region Init

        protected override void InitComponents()
        {
            base.InitComponents();

            if (_renderer == null) 
                _renderer = GetComponent<Renderer>();

            if (_connectorSelectable == null)
                _connectorSelectable = GetComponent<ConnectorSelectable>();
        }

        #endregion

        #region State & Play

        public void ToState(ConnectorAnimatorState state)
        {
            switch (State)
            {
                case ConnectorAnimatorState.Idle: Play(state); break;
                case ConnectorAnimatorState.Active: Play(state); break;
                case ConnectorAnimatorState.Delete: 
                    if (state == ConnectorAnimatorState.Restore) 
                        Play(ConnectorAnimatorState.Idle); 
                    break;
                case ConnectorAnimatorState.Restore: Play(state); break;
                case ConnectorAnimatorState.ReadyToConnect: Play(state); break;
                default: throw new NotImplementedException();
            }
        }

        private void Play(ConnectorAnimatorState state)
        {
            switch (state)
            {
                case ConnectorAnimatorState.Idle: PlayIdle(); break;
                case ConnectorAnimatorState.Active: PlayActive(); break;
                case ConnectorAnimatorState.Delete: PlayDelete(); break;
                case ConnectorAnimatorState.Restore: PlayRestore(); break;
                case ConnectorAnimatorState.ReadyToConnect: PlayReadyToConnect(); break;
                default: throw new NotImplementedException();
            }
        }

        protected override void PlayActive()
        {
            State = ConnectorAnimatorState.Active;
            _renderer.material = SelectedMaterial;
        }

        private void PlayReadyToConnect()
        {
            State = ConnectorAnimatorState.Active;
            _renderer.material = ActiveMaterial;
        }

        protected override void PlayIdle()
        {
            State = ConnectorAnimatorState.Idle;
            _renderer.material = _originalMaterial;
        }

        protected override void PlayDelete()
        {
            State = ConnectorAnimatorState.Delete;
            _renderer.material = DeleteMaterial;
        }

        protected override void PlayRestore()
        {
            State = ConnectorAnimatorState.Restore;
        }

        #endregion

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            SubscribeToPawnDraggable(subscribe);
            SubscribeToConnector(subscribe);
        }

        #endregion

        #region PawnDraggable

        private void SubscribeToPawnDraggable(bool subscribe)
        {
            if (_pawnDraggable == null)
                return;

            if (subscribe)
            {
                _pawnDraggable.onEnterCheckerboard += OnEnterCheckerboard;
                _pawnDraggable.onExitCheckerboard += OnExitCheckerboard;
            }
            else
            {
                _pawnDraggable.onEnterCheckerboard -= OnEnterCheckerboard;
                _pawnDraggable.onExitCheckerboard -= OnExitCheckerboard;
            }
        }

        private void OnEnterCheckerboard(PawnEnterCheckerboardEventArgs args)
        {
            ToState(ConnectorAnimatorState.Restore);
        }


        private void OnExitCheckerboard(PawnExitCheckerboardEventArgs args)
        {
            ToState(ConnectorAnimatorState.Delete);
        }

        #endregion

        #region Connector

        private void SubscribeToConnector(bool subscribe)
        {
            if (_connectorSelectable == null)
                return;

            if (subscribe)
            {
                _connectorSelectable.onSelectedChanged += OnSelectedChanged;
            }
            else
            {
                _connectorSelectable.onSelectedChanged -= OnSelectedChanged;
            }
        }

        private void OnSelectedChanged(SelectedChangedEventArgs args)
        {
            if (args.IsSelected)
                ToState(ConnectorAnimatorState.Active);
            else
                ToState(ConnectorAnimatorState.Idle);
        }

        #endregion
    }

    public abstract class AbstractConnectorAnimator : AbstractEntityAnimator
    {
        protected abstract void PlayActive();
        protected abstract void PlayDelete();
        protected abstract void PlayRestore();
    }
}
