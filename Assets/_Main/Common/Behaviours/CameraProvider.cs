using UnityEngine;

namespace Main.Common.Behaviours
{
    public sealed class CameraProvider : ICameraProvider
    {
        #region Fields

        private Camera _camera;

        #endregion

        #region Init

        public CameraProvider(Camera camera)
        {
            SetCamera(camera);
        }

        #endregion

        #region Methods

        public Camera GetCamera() => _camera;

        public void SetCamera(Camera camera) => this._camera = camera;

        #endregion
    }

    public interface ICameraProvider
    {
        Camera GetCamera();
        void SetCamera(Camera camera);
    }
}
