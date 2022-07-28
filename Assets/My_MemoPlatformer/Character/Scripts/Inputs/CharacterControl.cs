using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public enum TransitionParameter
    {
        Move,
    }


    public class CharacterControl : MonoBehaviour
    {
        public bool MoveRight;
        public bool MoveLeft;

        private Vector3 _cameraOffset = new Vector3(0, 2.0f, -12.0f);

        // [SerializeField] private Camera _Playercamera;
        [SerializeField] Animator _animator;



        private void SetCamera()
        {
            //  _Playercamera.transform.position = transform.position + _cameraOffset;
        }


        private void LateUpdate()
        {
            SetCamera();
        }

    }
}