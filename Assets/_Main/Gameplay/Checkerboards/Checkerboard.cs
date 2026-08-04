using UnityEngine;

namespace Main.Gameplay.Checkerboards
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Checkerboard : MonoBehaviour
    {
        private MeshRenderer meshRenderer;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void Initialize(CheckerboardInitParameters parameters)
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
    }
}
