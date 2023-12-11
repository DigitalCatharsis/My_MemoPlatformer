using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/Idle")]
    public class Idle : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Jump], false);
            animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Attack], false);
            animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Move], false);

            characterState.characterControl.animationProgress.disAllowEarlyTurn = false;
            characterState.characterControl.procDict[CharacterProc.CLEAR_FRONTBLOCKING_OBJ_DICTIONARY]();
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.animationProgress.lockDirectionNextState = false;

            if (characterState.characterControl.jump)
            {
                if (!characterState.characterControl.animationProgress.jumped)
                {
                    if (characterState.characterControl.animationProgress.ground != null)
                    {
                        animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Jump], true);
                    }
                }
            }
            else
            {
                if (!characterState.characterControl.animationProgress.IsRunning(typeof(Jump)))   //double update fix. Guess idle is overlapping jump or moveforward
                {
                    characterState.characterControl.animationProgress.jumped = false;
                }
            }

            //Moving
            if (characterState.characterControl.moveLeft && characterState.characterControl.moveRight)
            {
                //nothing to fix bug with double press
            }
            else if (characterState.characterControl.moveRight)
            {
                animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Move], true);
            }
            else if (characterState.characterControl.moveLeft)
            {
                animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Move], true);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            //animator.SetBool(TransitionParameter.Attack.ToString(), false);
        }
    }

}
