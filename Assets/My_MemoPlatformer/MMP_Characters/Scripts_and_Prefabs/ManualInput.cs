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
            if (VirtualInputManager.Instance.turbo)
            {
                characterControl.turbo = true;
            }
            else
            {
                characterControl.turbo = false;
            }

            if (VirtualInputManager.Instance.moveUp)
            {
                characterControl.moveUp = true;
            }
            else
            {
                characterControl.moveUp = false;
            }

            if (VirtualInputManager.Instance.moveDown)
            {
                characterControl.moveDown = true;
            }
            else
            {
                characterControl.moveDown = false;
            }

            if (VirtualInputManager.Instance.moveRight)
            {
                characterControl.moveRight = true;
            }
            else
            {
                characterControl.moveRight = false;
            }

            if (VirtualInputManager.Instance.moveLeft)
            {
                characterControl.moveLeft = true;
            }
            else
            {
                characterControl.moveLeft = false;
            }

            if (VirtualInputManager.Instance.jump)
            {
                characterControl.jump = true;
            }
            else
            {
                characterControl.jump = false;
            }

            if (VirtualInputManager.Instance.attack)
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