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

        private const string landingState = "Jump_Normal_Landing";

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {      
            characterState.characterControl.animationProgress.targetSize = targetSize;
            characterState.characterControl.animationProgress.sizeSpeed = sizeUpdateSpeed;

            characterState.characterControl.animationProgress.targetCenter = targetCenter;
            characterState.characterControl.animationProgress.centerSpeed = centerUpdateSpeed;

            if (stateInfo.IsName(landingState)) 
            {
                characterState.characterControl.animationProgress.isLanding = true;
            }

        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (stateInfo.IsName(landingState))
            {
                characterState.characterControl.animationProgress.isLanding = false;
            }
        }
    }
}