using System;
using Main.Common.Behaviours;
using Main.Gameplay.Data;
using UnityEngine;
using Zenject;

namespace Main.Gameplay.Animations
{
    public class PawnAnimator : AbstractPawnEntityAnimator
    {
        protected override void PlayActive()
        {
            State = PawnAnimatorState.Active;
        }
    }

    public abstract class AbstractPawnEntityAnimator : AbstractEntityAnimator
    {
        [SerializeField] protected Renderer renderer;

        private SceneData _sceneData;
        private PawnDraggable _pawnDraggable;
        private Material _originalMaterial;

        private Material ActiveMaterial => _sceneData.CrazyPawnSettings.ActiveConnectorMaterial;
        private Material DeleteMaterial => _sceneData.CrazyPawnSettings.DeleteMaterial;

        public PawnAnimatorState State { get; protected set; }

        [Inject]
        private void Construct(SceneData sceneData, PawnDraggable pawnDraggable)
        {
            this._sceneData = sceneData;
            this._pawnDraggable = pawnDraggable;
        }

        protected override void Awake()
        {
            base.Awake();
            _originalMaterial = renderer.material;
        }

        protected override void InitComponents()
        {
            base.InitComponents();

            if (renderer == null)
                renderer = GetComponent<Renderer>();
        }

        protected override void InitComponentsOnValidate()
        {
            base.InitComponentsOnValidate();

            if (renderer == null)
                renderer = GetComponent<Renderer>();
        }

        public virtual void ToState(PawnAnimatorState state)
        {
            switch (State)
            {
                case PawnAnimatorState.Idle: Play(state); break;
                case PawnAnimatorState.Active: Play(state); break;
                case PawnAnimatorState.Delete: if (state == PawnAnimatorState.Restore) PlayIdle(); break;
                case PawnAnimatorState.Restore: Play(state); break;
                default: throw new NotImplementedException();
            }
        }

        protected void Play(PawnAnimatorState state)
        {
            switch (state)
            {
                case PawnAnimatorState.Idle: PlayIdle(); break;
                case PawnAnimatorState.Active: PlayActive(); break;
                case PawnAnimatorState.Delete: PlayDelete(); break;
                case PawnAnimatorState.Restore: PlayRestore(); break;
                default: throw new NotImplementedException();
            }
        }

        protected override void PlayActive()
        {
            State = PawnAnimatorState.Active;
            renderer.material = ActiveMaterial;
        }

        protected override void PlayIdle()
        {
            State = PawnAnimatorState.Idle;
            renderer.material = _originalMaterial;
        }

        protected override void PlayDelete()
        {
            State = PawnAnimatorState.Delete;
            renderer.material = DeleteMaterial;
        }

        protected virtual void PlayRestore()
        {
            State = PawnAnimatorState.Restore;
        }

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            SubscribeToPawnDraggable(subscribe);
        }

        #endregion

        #region PawnDraggable

        protected void SubscribeToPawnDraggable(bool subscribe)
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
            ToState(PawnAnimatorState.Restore);
        }

        private void OnExitCheckerboard(PawnExitCheckerboardEventArgs args)
        {
            ToState(PawnAnimatorState.Delete);
        }

        #endregion

        public enum PawnAnimatorState { Idle, Active, Delete, Restore }
    }

    

    public abstract class AbstractEntityAnimator : AbstractMonoBehaviourExtended
    {
        protected abstract void PlayIdle();
        protected abstract void PlayActive();
        protected abstract void PlayDelete();
    }
}
