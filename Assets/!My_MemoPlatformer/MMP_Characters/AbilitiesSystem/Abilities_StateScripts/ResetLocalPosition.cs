using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/ResetLocalPosition")]
    public class ResetLocalPosition : CharacterAbility
    {
        public bool onStart;
        public bool onEnd;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (onStart)
            {
                
                characterState.characterControl.skinnedMeshAnimator.transform.localPosition = Vector3.zero;
                characterState.characterControl.skinnedMeshAnimator.transform.localRotation = Quaternion.identity;
            }

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (onEnd) 
            {
                
                characterState.characterControl.skinnedMeshAnimator.transform.localPosition = Vector3.zero;
                characterState.characterControl.skinnedMeshAnimator.transform.localRotation = Quaternion.identity;
            }   

        }


    }
}
