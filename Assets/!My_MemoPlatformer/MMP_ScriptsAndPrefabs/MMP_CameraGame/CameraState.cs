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


            for (int i = 0; i < (int)CameraTrigger.COUNT; i++)
            {
                CameraManager.Instance.CAM_CONTROLLER.ANIMATOR.
                    ResetTrigger(HashManager.Instance.ArrCameraParams[(int)i]);
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.normalizedTime > 0.7f)
            {
                if (stateInfo.IsName("Shake"))
                {
                    animator.SetTrigger(HashManager.Instance.ArrCameraParams[(int)CameraTrigger.Default]);
                }
            }
        }
    }
}