using Main.Common.Behaviours;
using Main.Gameplay.Data;
using Main.Gameplay.Pawns;
using Main.Gameplay.Pawns.Animations;
using System;
using UnityEngine;
using Zenject;
using static Main.Gameplay.Connectors.PawnConnectorAnimator;

namespace Main.Gameplay.Animations
{
    public sealed class PawnAnimator : AbstractPawnAnimator
    {
        public enum PawnAnimatorState { Idle, Active, Delete, Restore }

        [SerializeField] private PawnSelectable _selectable;
        [SerializeField] private Renderer _renderer;

        private SceneData _sceneData;
        private PawnDraggable _pawnDraggable;
        private Material _originalMaterial;

        private Material ActiveMaterial => _sceneData.CrazyPawnSettings.ActiveConnectorMaterial;
        private Material SelectedMaterial => _sceneData.Settings.SelectedMaterial;
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
            _originalMaterial = _renderer.material;
        }

        protected override void InitComponents()
        {
            base.InitComponents();

            if (_renderer == null)
                _renderer = GetComponent<Renderer>();

            if (_selectable == null)
                _selectable = GetComponent<PawnSelectable>();
        }

        public void ToState(PawnAnimatorState state)
        {
            switch (State)
            {
                case PawnAnimatorState.Idle: Play(state); break;
                case PawnAnimatorState.Active: Play(state); break;
                case PawnAnimatorState.Delete: 
                    if (state == PawnAnimatorState.Restore)
                        if (_selectable.IsSelected) Play(PawnAnimatorState.Active);
                        else Play(PawnAnimatorState.Idle);
                    break;
                case PawnAnimatorState.Restore: Play(state); break;
                default: throw new NotImplementedException();
            }
        }

        private void Play(PawnAnimatorState state)
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
            _renderer.material = SelectedMaterial;
        }

        protected override void PlayIdle()
        {
            State = PawnAnimatorState.Idle;
            _renderer.material = _originalMaterial;
        }

        protected override void PlayDelete()
        {
            State = PawnAnimatorState.Delete;
            _renderer.material = DeleteMaterial;
        }

        protected override void PlayRestore()
        {
            State = PawnAnimatorState.Restore;
        }

        #region Subscribe

        protected override void SubscribeInner(bool subscribe)
        {
            SubscribeToPawnDraggable(subscribe);
            SubscribeToPawn(subscribe);
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
            ToState(PawnAnimatorState.Restore);
        }


        private void OnExitCheckerboard(PawnExitCheckerboardEventArgs args)
        {
            ToState(PawnAnimatorState.Delete);
        }

        #endregion

        #region Pawn

        private void SubscribeToPawn(bool subscribe)
        {
            if (_selectable == null)
                return;

            if (subscribe)
            {
                _selectable.onSelectedChanged += OnSelectedChanged;
            }
            else
            {
                _selectable.onSelectedChanged -= OnSelectedChanged;
            }
        }

        private void OnSelectedChanged(SelectedChangedEventArgs args)
        {
            if (args.IsSelected)
                ToState(PawnAnimatorState.Active);
            else
                ToState(PawnAnimatorState.Idle);
        }

        #endregion
    }
}
