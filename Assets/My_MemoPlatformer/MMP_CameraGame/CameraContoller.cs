using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public enum CameraTrigger
    {
        Default,
        Shake,
    }


    public class CameraContoller : MonoBehaviour
    {

        private Animator _animator;
        public Animator Animator
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
            Animator.SetTrigger(HashManager.Instance.dicCameraTriggers[trigger]);
        }

    }
}