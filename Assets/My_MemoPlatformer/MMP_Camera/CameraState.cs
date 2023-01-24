using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class CameraState : StateMachineBehaviour
    {
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            CameraTrigger[] arr = System.Enum.GetValues(typeof(CameraTrigger)) as CameraTrigger[];


            foreach (CameraTrigger t in arr)
            {
                CameraManager.Instance.Cam_Controller.Animator.ResetTrigger(t.ToString());
            }
        }
    }
}