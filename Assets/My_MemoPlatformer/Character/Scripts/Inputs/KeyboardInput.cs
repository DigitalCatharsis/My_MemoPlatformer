using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class KeyboardInput : MonoBehaviour
    {
        private NIS_PlayerControls _controls;

        private float _direction;
        private void Awake()
        {
            _controls = new NIS_PlayerControls();
            _controls.Enable();

            _controls.Land.Move.performed += ctx =>
            {
                _direction = ctx.ReadValue<float>();
            };

            //_controls.Land.Jump.performed += ctx => Jump();                       
        }
        private void Update()
        {

            if (_direction > 0 && _direction != 0)
            {
                VirtualInputManager.Instance.MoveRight = true;
            }
            else
            {
                VirtualInputManager.Instance.MoveRight = false;
            }

            if (_direction < 0 && _direction != 0)
            {
                VirtualInputManager.Instance.MoveLeft = true;
            }
            else
            {
                VirtualInputManager.Instance.MoveLeft = false;
            }
            //if (_direction == 0)
            //{
            //    VirtualInputManager.Instance.MoveRight = false;
            //    VirtualInputManager.Instance.MoveLeft = false;
            //}
        }
    }
}

