using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/UpdateBoxCollider")]
    public class UpdateBoxCollider : StateData
    {
        public Vector3 targetCenter;
        public float centerUpdateSpeed;
        [Space(10)]
        public Vector3 targetSize;
        public float sizeUpdateSpeed;
        [Space(10)]
        public bool keepUpdating;



        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
            characterState.characterControl.animationProgress.updatingBoxCollider = true;

            characterState.characterControl.animationProgress.targetSize = targetSize;
            characterState.characterControl.animationProgress.sizeSpeed = sizeUpdateSpeed;

            characterState.characterControl.animationProgress.targetCenter = targetCenter;
            characterState.characterControl.animationProgress.centerSpeed = centerUpdateSpeed;

        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            

            if (!keepUpdating) 
            {
                characterState.characterControl.animationProgress.updatingBoxCollider = false;
            }            
        }
    }
}