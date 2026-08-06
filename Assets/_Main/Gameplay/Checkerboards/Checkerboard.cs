using Cysharp.Threading.Tasks;
using Main.Common.Extensions;
using UnityEngine;

namespace Main.Gameplay.Checkerboards
{
    [RequireComponent(typeof(MeshRenderer))]
    public sealed class Checkerboard : MonoBehaviour
    {
        private MeshRenderer _renderer;
        private Collider _collider;

        private void Awake()
        {
            if (_renderer == null) _renderer = GetComponent<MeshRenderer>();
            if (_collider == null) _collider = GetComponent<Collider>();
        }

        public async UniTask InitializeAsync(CheckerboardInitParameters parameters)
        {
            // здесь мог бы быть истинно асинхронный метод, но есть такой без await - просто в качестве примера,
            // потому что в рамках задачи и разработанного решения для других асинхронных методов места не нашлось :-(

            if (parameters.boardSize < 1)
                parameters.boardSize = 1;

            float finalSize = parameters.boardSize * parameters.cellSize;

            transform.localScale = new Vector3(finalSize, finalSize, 1);

            Material checkerMaterial = _renderer.material;

            checkerMaterial.SetFloat("_CellCountPerRow", parameters.boardSize);
            checkerMaterial.SetFloat("_CellCountPerColumn", parameters.boardSize);
            checkerMaterial.SetColor("_CellColorA", parameters.WhiteCellColor);
            checkerMaterial.SetColor("_CellColorB", parameters.BlackCellColor);
        }

        public bool IsInside(Vector3 point) => 
            _collider.IsInside(point);
    }
}
