using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

namespace My_MemoPlatformer
{

    public class CameraManager : Singleton<CameraManager>
    {
        public Camera mainCamera;

        private Coroutine _routine;


        private CameraContoller _cameraController;
        public CameraContoller CAM_CONTROLLER
        {
            get
            {
                if (_cameraController == null)
                {
                    _cameraController = GameObject.FindObjectOfType<CameraContoller>();
                }
                return _cameraController;
            }
        }

        private void Awake()
        {
            GameObject obj = GameObject.Find("Main Camera");
            mainCamera = obj.GetComponent<Camera>(); ;
        }

        //Controlling how long camera shake is. After some amount of second return to default camera
        IEnumerator _CamShake(float sec)
        {
            CAM_CONTROLLER.TriggerCamera(CameraTrigger.Shake);
            yield return new WaitForSeconds(sec);
            CAM_CONTROLLER.TriggerCamera(CameraTrigger.Default);
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