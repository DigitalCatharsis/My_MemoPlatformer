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
            if (characterState.characterControl.PlayerRotation_Data.IsFacingForward())
            {
                if (characterState.characterControl.moveLeft)
                {
                    animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Turn], true);
                }
            }

            if (!characterState.characterControl.PlayerRotation_Data.IsFacingForward())
            {
                if (characterState.characterControl.moveRight)
                {
                    animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Turn], true);
                }
            }
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Turn], false);
        }


    }
}