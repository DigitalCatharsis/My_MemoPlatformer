using UnityEngine;

namespace My_MemoPlatformer
{
    public class CameraController : MonoBehaviour
    {
        public Cinemachine.CinemachineVirtualCamera defaultCam;
        public float zoomOutSpeed;
        public float zoomInSpeed;

        private Animator _animator;
        public Animator ANIMATOR
        {
            get
            {
                if (_animator == null)
                {
                    _animator = GetComponent<Animator>();
                }
                return _animator;
            }
        }

        public void TriggerCamera(CameraTrigger trigger)
        {
            ANIMATOR.SetTrigger(HashManager.Instance.arrCameraParams[(int)trigger]);
        }

    }
}