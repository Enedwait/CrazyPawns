using Cysharp.Threading.Tasks;
using Main.Common.Extensions;
using UnityEngine;

namespace Main.Gameplay.Checkerboards
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Checkerboard : MonoBehaviour
    {
        private Renderer meshRenderer;
        private Collider collider;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            collider = GetComponent<Collider>();
        }

        public async UniTask InitializeAsync(CheckerboardInitParameters parameters)
        {
            if (parameters.boardSize < 1)
                parameters.boardSize = 1;

            float finalSize = parameters.boardSize * parameters.cellSize;

            transform.localScale = new Vector3(finalSize, finalSize, 1);

            Material checkerMaterial = meshRenderer.material;

            checkerMaterial.SetFloat("_CellCountPerRow", parameters.boardSize);
            checkerMaterial.SetFloat("_CellCountPerColumn", parameters.boardSize);
            checkerMaterial.SetColor("_CellColorA", parameters.WhiteCellColor);
            checkerMaterial.SetColor("_CellColorB", parameters.BlackCellColor);
        }

        public bool IsInside(Vector3 point) => 
            collider.IsInside(point);
    }
}
