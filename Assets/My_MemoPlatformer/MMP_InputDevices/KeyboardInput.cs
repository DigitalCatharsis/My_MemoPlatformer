using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class KeyboardInput : MonoBehaviour
    {
        #region Input system (old)
        //    private NIS_PlayerControls _controls;

        //    private float _direction;

        //    private float _jump;
        //    private void Awake()
        //    {            
        //        _controls = new NIS_PlayerControls();
        //        _controls.Enable();

        //        _controls.Land.Move.performed += ctx =>
        //        {
        //            _direction = ctx.ReadValue<float>();
        //        };

        //        _controls.Land.Jump.performed += ctx => 
        //        {

        //            _jump = ctx.ReadValue<float>();
        //            Debug.Log("DIRECTION:  " + _jump);
        //            if (_jump > 0)
        //            {
        //                VirtualInputManager.Instance.Jump = true;
        //            }
        //            else
        //            {
        //                VirtualInputManager.Instance.Jump = false;
        //            }
        //            Debug.Log(_jump);
        //        };                  
        //    }

        //    private void Update()
        //    {
        //        Debug.Log("DIRECTION:  " + _direction);
        //        if (_direction > 0 && _direction != 0)
        //        {
        //            VirtualInputManager.Instance.MoveRight = true;
        //        }
        //        else
        //        {
        //            VirtualInputManager.Instance.MoveRight = false;
        //        }

        //        if (_direction < 0 && _direction != 0)
        //        {
        //            VirtualInputManager.Instance.MoveLeft = true;
        //        }
        //        else
        //        {
        //            VirtualInputManager.Instance.MoveLeft = false;
        //        }



        //    }

        //    private void SetJump(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        //    {

        //    }

        //    private void OnEnable()
        //    {
        //        _controls.Enable();
        //    }

        //    private void OnDisable()
        //    {
        //        _controls.Disable();
        //    }
        #endregion

        void Update()
        {
            if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.LeftShift))
            {
                VirtualInputManager.Instance.turbo = true;
            }
            else
            {
                VirtualInputManager.Instance.turbo = false;
            }

            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                VirtualInputManager.Instance.moveUp = true;
            }
            else
            {
                VirtualInputManager.Instance.moveUp = false;
            }

            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                VirtualInputManager.Instance.moveDown = true;
            }
            else
            {
                VirtualInputManager.Instance.moveDown = false;
            }

            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                VirtualInputManager.Instance.moveRight = true;
            }
            else
            {
                VirtualInputManager.Instance.moveRight = false;
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                VirtualInputManager.Instance.moveLeft = true;
            }
            else
            {
                VirtualInputManager.Instance.moveLeft = false;
            }

            if (Input.GetKey(KeyCode.Space))
            {
                VirtualInputManager.Instance.jump = true;
            }
            else
            {
                VirtualInputManager.Instance.jump = false;
            }

            //    if (Input.GetKeyDown(KeyCode.C))
            if (Input.GetKey(KeyCode.C))
            {
                VirtualInputManager.Instance.attack = true;
            }
            else
            {
                VirtualInputManager.Instance.attack = false;
            }
        }

    }
}

