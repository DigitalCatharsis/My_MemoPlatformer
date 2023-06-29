using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/CheckTurbo")]
    public class CheckTurbo : StateData
    {

        public bool mustRequireMovement;
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            

            if (characterState.characterControl.turbo)
            {
                if (mustRequireMovement)
                {
                    if (characterState.characterControl.moveLeft || characterState.characterControl.moveRight)
                    {
                        animator.SetBool(TransitionParameter.Turbo.ToString(), true);
                    }
                    else
                    {
                        animator.SetBool(TransitionParameter.Turbo.ToString(), false);
                    }
                }
                else
                {
                    animator.SetBool(TransitionParameter.Turbo.ToString(), true);
                }
            }
            else
            {
                animator.SetBool(TransitionParameter.Turbo.ToString(), false);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(TransitionParameter.Attack.ToString(), false);
        }
    }

}
