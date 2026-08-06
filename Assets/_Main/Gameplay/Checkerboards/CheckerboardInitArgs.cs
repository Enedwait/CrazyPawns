using UnityEngine;

namespace Main.Gameplay.Checkerboards
{
    public record CheckerboardInitArgs(int BoardSize, float CellSize, Color WhiteCellColor, Color BlackCellColor);
}
