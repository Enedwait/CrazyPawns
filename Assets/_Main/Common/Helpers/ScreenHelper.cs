using UnityEngine;

namespace Main.Common.Helpers
{
    public static class ScreenHelper
    {
        public static Vector2 GetScreenCenter() =>
            new Vector2(Screen.width / 2f, Screen.height / 2f);
    }
}
