using UnityEngine;

namespace Main.Gameplay.Managers.Pan
{
    public sealed class PanTarget : MonoBehaviour, IPanTarget
    {
        #region Fields

        [SerializeField, Range(0.001f, 10f)] private float _panSpeed = 1.75f;

        private Vector2 _panDelta;

        #endregion

        #region Unity Methods

        private void Update()
        {
            if (!enabled)
                return;

            float deltaTime = Time.deltaTime;

            Pan(in deltaTime);
        }

        #endregion

        #region Pan

        public void SetPan(Vector2 panDelta)
        {
            _panDelta = panDelta;
        }

        private void Pan(in float deltaTime)
        {
            transform.Translate(
                new Vector3(_panDelta.x, 0, _panDelta.y) * _panSpeed * deltaTime, 
                Space.World);
        }

        #endregion
    }
}
