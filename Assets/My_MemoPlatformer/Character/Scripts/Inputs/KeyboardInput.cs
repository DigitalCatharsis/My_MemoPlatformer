using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class KeyboardInput : MonoBehaviour
    {
        private NIS_PlayerControls _controls;

        private float _direction;

        private float _jump;
        private void Awake()
        {            
            _controls = new NIS_PlayerControls();
            _controls.Enable();

            _controls.Land.Move.performed += ctx =>
            {
                _direction = ctx.ReadValue<float>();
            };

            _controls.Land.Jump.performed += ctx =>
            {
                _jump = ctx.ReadValue<float>();
            };                  
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

            if (_jump > 0)
            {
                VirtualInputManager.Instance.Jump = true;
            }
            else
            {
                VirtualInputManager.Instance.Jump = false;
            }

        }



    }
}

