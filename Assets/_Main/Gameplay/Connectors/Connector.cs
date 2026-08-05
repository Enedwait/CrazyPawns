using Main.Common.Behaviours;

namespace Main.Gameplay.Connectors
{
    public class Connector : AbstractSelectable
    {
        /*
        private PawnAnimator _pawnAnimator;

        [Inject]
        private void Construct(PawnAnimator pawnAnimator)
        {
            this._pawnAnimator = pawnAnimator;
        }*/

        /*
        [SerializeField] private Renderer renderer;

        private SceneData _sceneData;
        private PawnDraggable _pawnDraggable;
        private Material _originalMaterial;

        private Material ActiveMaterial => _sceneData.CrazyPawnSettings.ActiveConnectorMaterial;
        private Material DeleteMaterial => _sceneData.CrazyPawnSettings.DeleteMaterial;

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
        }*/

        protected override bool SelectInner()
        {
            //SetActive();
            return true;
        }

        protected override bool DeselectInner()
        {
            //SetDefault();
            return true;
        }
        /*
        private void SetActive()
        {
            renderer.material = ActiveMaterial;
        }

        private void SetDefault()
        {
            renderer.material = _originalMaterial;
        }

        private void SetDelete()
        {
            renderer.material = DeleteMaterial;
        }

        protected override void Subscribe(bool subscribe)
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
            SetDefault();
        }

        private void OnExitCheckerboard(PawnExitCheckerboardEventArgs args)
        {
            SetDelete();
        }*/

        protected override void SubscribeInner(bool subscribe)
        { }
    }
}
