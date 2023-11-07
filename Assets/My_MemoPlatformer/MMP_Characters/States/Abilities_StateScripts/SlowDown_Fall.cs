using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/SlowDown_Fall")]
    public class SlowDown_Fall : StateData
    {
        public Vector3 maxFallVelocity;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.animationProgress.maxFallVelocity = maxFallVelocity;
            characterState.characterControl.animationProgress.canWallJump = false;
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!characterState.characterControl.jump)
            {
                characterState.characterControl.animationProgress.canWallJump = true;
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.animationProgress.maxFallVelocity = Vector3.zero;
        }
    }

}
