using UnityEngine;
using UnityEngine.Events;

namespace Main.Common.Behaviours
{
    public sealed class CameraProvider : ICameraProvider
    {
        #region Fields

        private Camera _camera;

        #endregion

        #region Events

        public event UnityAction<Camera> onCameraChanged;

        #endregion

        #region Init

        public CameraProvider(Camera camera)
        {
            SetCamera(camera);
        }

        #endregion

        #region Methods

        public Camera GetCamera() => _camera;

        public void SetCamera(Camera camera)
        {
            this._camera = camera;
            RaiseOnCameraChanged();
        }

        #endregion

        #region Event Raisers

        private void RaiseOnCameraChanged() => 
            onCameraChanged?.Invoke(_camera);

        #endregion
    }

    public interface ICameraProvider
    {
        public event UnityAction<Camera> onCameraChanged;

        Camera GetCamera();
        void SetCamera(Camera camera);
    }
}
