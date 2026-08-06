using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Main.Gameplay.Checkerboards
{
    public interface ICheckerboard
    {
        UniTask InitializeAsync(CheckerboardInitArgs args);

        bool IsInside(Vector3 point);
    }
}
