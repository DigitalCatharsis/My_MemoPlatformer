using System.Collections;
using UnityEngine;

namespace My_MemoPlatformer
{

    public class CameraManager : Singleton<CameraManager>
    {
        public Camera mainCamera;
        private Coroutine _routine;
        private bool _camControllerInitiated = false;
        private CameraController _cameraController;
        public CameraController CAM_CONTROLLER
        {
            get
            {
                if (!_camControllerInitiated)
                {
                    _cameraController = GameObject.FindObjectOfType<CameraController>();
                    _camControllerInitiated = true;
                }
                return _cameraController;
            }
        }

        private void Awake()
        {
            var obj = GameObject.Find("Main Camera");
            mainCamera = obj.GetComponent<Camera>(); ;
        }

        //Controlling how long camera shake is. After some amount of second return to default camera
        IEnumerator _CamShake(float sec)
        {
            if (CAM_CONTROLLER != null)
            {
                CAM_CONTROLLER.TriggerCamera(CameraTrigger.Shake);
                yield return new WaitForSeconds(sec);
                CAM_CONTROLLER.TriggerCamera(CameraTrigger.Default);
            }
        }

        public void ShakeCamera(float sec)
        {
            if (_routine != null) 
            {
                StopCoroutine(_routine);
            }

            _routine = StartCoroutine(_CamShake (sec));
        }
    }
}