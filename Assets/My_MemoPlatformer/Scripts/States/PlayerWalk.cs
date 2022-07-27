using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyMemoPlatformer
{
    public class PlayerWalk : CharacterStateBase
    {
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Move(animator);
            Flip(animator);
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

        private void Move(Animator animator)
        {
            if (GetCharacterControl(animator).Direction != 0)
            {
                GetCharacterControl(animator).transform.Translate(Vector3.forward * GetCharacterControl(animator)._speed * Time.deltaTime);
            }
            else
            {
                animator.SetBool(TransitionParameter.Move.ToString(), false);
                return;
            }
        }

        private void Flip(Animator animator)
        {
            if (GetCharacterControl(animator).Direction < 0)
            {
                GetCharacterControl(animator).transform.rotation = Quaternion.Euler(0f, 180, 0f);
            }
            else if (GetCharacterControl(animator).Direction > 0)
            {
                GetCharacterControl(animator).transform.rotation = Quaternion.Euler(0f, 0, 0f);
            }

        }

    }
}
