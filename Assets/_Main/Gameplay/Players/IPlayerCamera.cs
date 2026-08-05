using UnityEngine;

namespace Main.Gameplay.Cameras
{
    public interface ICameraProvider
    {
        Camera GetCamera();
    }

    public class CameraProviderProvider : ICameraProvider
    {
        protected Camera camera;

        public CameraProviderProvider(Camera camera)
        {
            SetCamera(camera);
        }

        public Camera GetCamera() => camera;

        public Camera SetCamera(Camera camera) => 
            this.camera = camera;
    }
}
