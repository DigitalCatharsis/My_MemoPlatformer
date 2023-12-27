using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class CameraContoller : MonoBehaviour
    {

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
            ANIMATOR.SetTrigger(HashManager.Instance.ArrCameraParams[(int)trigger]);
        }

    }
}