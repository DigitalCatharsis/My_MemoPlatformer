using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class KeyboardInput : MonoBehaviour
    {
        #region Input system
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
            if (Input.GetKey(KeyCode.D))
            {
                VirtualInputManager.Instance.MoveRight = true;
            }
            else
            {
                VirtualInputManager.Instance.MoveRight = false;
            }

            if (Input.GetKey(KeyCode.A))
            {
                VirtualInputManager.Instance.MoveLeft = true;
            }
            else
            {
                VirtualInputManager.Instance.MoveLeft = false;
            }

            if (Input.GetKey(KeyCode.Space))
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

