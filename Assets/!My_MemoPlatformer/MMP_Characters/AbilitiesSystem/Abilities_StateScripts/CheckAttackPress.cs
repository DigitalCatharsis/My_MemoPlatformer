using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/CheckAttackPress")]
    public class CheckAttackPress : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (characterState.characterControl.attack)
            {
                characterState.characterControl.skinnedMeshAnimator.SetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.Attack], true);
            }
            else
            {
                characterState.characterControl.skinnedMeshAnimator.SetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.Attack], false);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }
    }

}