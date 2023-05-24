using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/CheckRunningTurn")]
    public class CheckRunningTurn : StateData
    {


        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);

            if (control.IsFacingForward())
            {
                if (control.moveLeft)
                {
                    animator.SetBool(TransitionParameter.Turn.ToString(), true);
                }
            }

            if (!control.IsFacingForward())
            {
                if (control.moveRight)
                {
                    animator.SetBool(TransitionParameter.Turn.ToString(), true);
                }
            }
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(TransitionParameter.Turn.ToString(), false);
        }


    }
}