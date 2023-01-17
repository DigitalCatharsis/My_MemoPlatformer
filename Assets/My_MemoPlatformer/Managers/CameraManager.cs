using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace My_MemoPlatformer
{

    public class CameraManager : Singleton<CameraManager>
    {
        private CameraContoller _cameraController;
        public CameraContoller Cam_Controller
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

        private Coroutine _routine;

        //Controlling how long camera shake is. After some amount of second return to default camera
        IEnumerator _CamShake(float sec)
        {
            Cam_Controller.TriggerCamera(CameraTrigger.Shake);
            yield return new WaitForSeconds(sec);
            Cam_Controller.TriggerCamera(CameraTrigger.Default);
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