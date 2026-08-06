using Cysharp.Threading.Tasks;
using Main.Common.Extensions;
using UnityEngine;

namespace Main.Gameplay.Checkerboards
{
    [RequireComponent(typeof(MeshRenderer))]
    public sealed class Checkerboard : MonoBehaviour, ICheckerboard
    {
        #region Fields

        private MeshRenderer _renderer;
        private Collider _collider;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (_renderer == null) _renderer = GetComponent<MeshRenderer>();
            if (_collider == null) _collider = GetComponent<Collider>();
        }

        #endregion

        #region Init

        public async UniTask InitializeAsync(CheckerboardInitArgs args)
        {
            // здесь мог бы быть истинно асинхронный метод, но есть такой без await - просто в качестве примера,
            // потому что в рамках задачи и разработанного решения для других асинхронных методов места не нашлось :-(

            float boardSize = args.BoardSize;
            if (boardSize < 1) boardSize = 1;

            float cellSize = args.CellSize;
            if (cellSize < 0.1f)
                cellSize = 0.1f;

            float finalSize = boardSize * cellSize;

            transform.localScale = new Vector3(finalSize, finalSize, 1);

            Material checkerMaterial = _renderer.material;

            checkerMaterial.SetFloat("_CellCountPerRow", boardSize);
            checkerMaterial.SetFloat("_CellCountPerColumn", boardSize);
            checkerMaterial.SetColor("_CellColorA", args.WhiteCellColor);
            checkerMaterial.SetColor("_CellColorB", args.BlackCellColor);
        }

        #endregion

        #region Methods

        public bool IsInside(Vector3 point) => 
            _collider.IsInside(point);

        #endregion
    }
}
