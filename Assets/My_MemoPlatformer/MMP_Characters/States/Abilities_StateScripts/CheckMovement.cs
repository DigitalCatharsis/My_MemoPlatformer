using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/ChecMovement")]
    public class ChecMovement : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            

            if (characterState.characterControl.moveLeft || characterState.characterControl.moveRight)
            {
                animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Move], true);
            }
            else
            {
                animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Move], false);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }
    }

}
