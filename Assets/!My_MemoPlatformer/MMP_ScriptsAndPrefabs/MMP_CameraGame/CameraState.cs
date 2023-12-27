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
                CameraManager.Instance.Cam_Controller.Animator.ResetTrigger(HashManager.Instance.dicCameraTriggers[t]);
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.normalizedTime > 0.7f)
            {
                if (stateInfo.IsName("Shake"))
                {
                    animator.SetTrigger(HashManager.Instance.dicCameraTriggers[CameraTrigger.Default]);
                }
            }
        }
    }
}