using System;
using UnityEngine;

namespace Main.Gameplay.Checkerboards
{
    [Serializable]
    public struct CheckerboardInitParameters
    {
        public int boardSize;
        public float cellSize;
        public Color WhiteCellColor;
        public Color BlackCellColor;
    }
}
