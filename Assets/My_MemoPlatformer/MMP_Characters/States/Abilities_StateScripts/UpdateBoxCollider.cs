using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/UpdateBoxCollider")]
    public class UpdateBoxCollider : StateData
    {

        public Vector3 targetSize;
        public Vector3 targetCenter;

        public float sizeUpdateSpeed;
        public float centerUpdateSpeed;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);
            control.animationProgress.isUpdatingBoxCollider = true;

            control.animationProgress.targetCenter = targetCenter;
            control.animationProgress.targetSize = targetSize;
            control.animationProgress.sizeSpeed = sizeUpdateSpeed;
            control.animationProgress.centerSpeed = centerUpdateSpeed;

        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);
            control.animationProgress.isUpdatingBoxCollider = false;
        }
    }
}