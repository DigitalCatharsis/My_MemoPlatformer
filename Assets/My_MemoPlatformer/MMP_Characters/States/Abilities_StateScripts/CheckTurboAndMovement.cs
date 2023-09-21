using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/CheckTurboAndMovement")]
    public class CheckTurboAndMovement : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            

            if ((characterState.characterControl.moveLeft || characterState.characterControl.moveRight) & characterState.characterControl.turbo)
            {
                animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Move], true);
                animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Turbo], true);
            }
            else
            {
                animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Move], false);
                animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Turbo], false);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
    }

}
