using Unity.Mathematics;

namespace Main.Common.Extensions
{
    public static class Extensions
    {
        public static Random InitializeRandom(this object someObject)
        {
            uint seed = (uint)System.DateTime.UtcNow.Ticks;
            if (seed == 0) seed = 1;
            return new Random(seed);
        }
    }
}
