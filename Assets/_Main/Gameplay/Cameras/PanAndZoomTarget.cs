using UnityEngine;

namespace Main.Gameplay.Cameras
{
    public sealed class PanAndZoomTarget : MonoBehaviour
    {
        #region Serialize Fields

        [SerializeField, Range(0.001f, 10f)] private float _panSpeed = 1.75f;
        [SerializeField, Range(0.001f, 10f)] private float _zoomSpeed = 0.27f;

        #endregion

        #region Fields

        private Vector2 _panDelta;
        private Vector3 _zoomDirection;
        private float _zoomDelta;

        #endregion

        #region Unity Methods

        private void Update()
        {
            if (!enabled)
                return;

            float deltaTime = Time.deltaTime;

            PanCamera(in deltaTime);
            ZoomCamera(in deltaTime);
        }

        #endregion

        #region Methods

        public void SetPan(Vector2 delta)
        {
            _panDelta = delta;
        }

        public void SetZoom(Vector3 zoomDirection, float delta)
        {
            _zoomDelta = delta;
            _zoomDirection = zoomDirection;
        }

        #endregion

        #region Pan

        private void PanCamera(in float deltaTime)
        {
            transform.Translate(
                new Vector3(_panDelta.x, 0, _panDelta.y) * _panSpeed * deltaTime, 
                Space.World);
        }

        #endregion

        #region Zoom

        private void ZoomCamera(in float deltaTime)
        {
            transform.Translate(
                _zoomDirection * _zoomDelta * _zoomSpeed * deltaTime,
                Space.World); 
        }

        #endregion
    }
}
