using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/ForceTransition")]
    public class ForceTransition : CharacterAbility
    {
        [Range(0.01f, 1f)]
        public float transitionTiming;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            // AnimatorStateInfo.normalizedTime  - The integer part is the number of time a state has been looped. The fractional part is the % (0-1) of progress in the current loop.
            if (stateInfo.normalizedTime >= transitionTiming)
            {
                animator.SetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.ForceTransition], true);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.ForceTransition], false);
        }
    }

}