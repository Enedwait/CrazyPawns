using UnityEngine;

namespace Main.Gameplay.Managers.Zoom
{
    public sealed class ZoomTarget : MonoBehaviour, IZoomTarget
    {
        #region Fields

        [SerializeField, Range(0.001f, 10f)] private float _zoomSpeed = 0.27f;

        private Vector3 _zoomDirection;
        private float _zoomDelta;

        #endregion

        #region Unity Methods

        private void Update()
        {
            if (!enabled)
                return;

            float deltaTime = Time.deltaTime;
            Zoom(in deltaTime);
        }

        #endregion

        #region Zoom

        public void SetZoom(Vector3 zoomDirection, float zoomDelta)
        {
            _zoomDelta = zoomDelta;
            _zoomDirection = zoomDirection;
        }

        private void Zoom(in float deltaTime)
        {
            transform.Translate(
                _zoomDirection * _zoomDelta * _zoomSpeed * deltaTime,
                Space.World);
        }

        #endregion
    }
}