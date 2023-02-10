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
            CharacterControl control = characterState.GetCharacterControl(animator);

            if ((control.moveLeft || control.moveRight) & control.turbo)
            {
                animator.SetBool(TransitionParameter.Move.ToString(), true);
                animator.SetBool(TransitionParameter.Turbo.ToString(), true);
            }
            else
            {
                animator.SetBool(TransitionParameter.Move.ToString(), false);
                animator.SetBool(TransitionParameter.Turbo.ToString(), false);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
    }

}
