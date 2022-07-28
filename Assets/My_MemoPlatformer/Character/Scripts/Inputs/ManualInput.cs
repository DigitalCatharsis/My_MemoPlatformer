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
                characterControl.MoveRight = true;
            }
            else
            {
                characterControl.MoveRight = false;
            }

            if (VirtualInputManager.Instance.MoveLeft)
            {
                characterControl.MoveLeft = true;
            }
            else
            {
                characterControl.MoveLeft = false;
            }
        }
    }

}