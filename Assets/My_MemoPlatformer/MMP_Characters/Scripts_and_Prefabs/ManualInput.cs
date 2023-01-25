using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{
    public class ManualInput : MonoBehaviour
    {
    private CharacterControl characterControl;

        private void Awake()
        {
            characterControl = this.gameObject.GetComponent<CharacterControl>();    
        }

        private void Update()
        {
            if (VirtualInputManager.Instance.MoveRight)
            {
                characterControl.moveRight = true;
            }
            else
            {
                characterControl.moveRight = false;
            }

            if (VirtualInputManager.Instance.MoveLeft)
            {
                characterControl.moveLeft = true;
            }
            else
            {
                characterControl.moveLeft = false;
            }

            if (VirtualInputManager.Instance.Jump)
            {
                characterControl.jump = true;
            }
            else
            {
                characterControl.jump = false;
            }

            if (VirtualInputManager.Instance.Attack)
            {
                characterControl.attack = true;
            }
            else
            {
                characterControl.attack = false;
            }

        }
    }

}