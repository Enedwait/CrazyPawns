using UnityEngine;
using Random = Unity.Mathematics.Random;

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

        public static RaycastHit GetClosestHit(this RaycastHit[] hits, int maxLen)
        {
            RaycastHit hit = default;
            float minDistance = float.MaxValue;
            for (int i = 0; i < maxLen; i++)
            {
                if (minDistance > hits[i].distance)
                {
                    hit = hits[i];
                    minDistance = hit.distance;
                }
            }
            return hit;
        }

        public static Vector3 GetPointAlongRayWithY(this Ray ray, float targetY)
        {
            if (Mathf.Approximately(ray.direction.y, 0f))
                return Vector3.zero;

            return ray.GetPoint((targetY - ray.origin.y) / ray.direction.y);
        }

        public static bool IsInside(this Collider collider, Vector3 point) =>
            collider.ClosestPoint(point) == point;
        
        public static bool IsNullOrDestroyed(this object target)
        {
            if (target is UnityEngine.Object unityObject) return unityObject == null;
            return target == null;
        }
    }
}


