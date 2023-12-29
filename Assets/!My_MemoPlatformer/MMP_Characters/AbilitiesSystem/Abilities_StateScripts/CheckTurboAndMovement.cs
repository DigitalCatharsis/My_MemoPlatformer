using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/CheckTurboAndMovement")]
    public class CheckTurboAndMovement : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if ((characterState.characterControl.moveLeft || characterState.characterControl.moveRight) &&
                characterState.characterControl.turbo)
            {
                animator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Move], true);
                animator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Turbo], true);
            }
            else
            {
                animator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Move], false);
                animator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Turbo], false);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
    }

}
