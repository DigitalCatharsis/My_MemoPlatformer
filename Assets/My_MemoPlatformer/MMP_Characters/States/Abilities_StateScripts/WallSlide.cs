using My_MemoPlatformer.Datasets;
using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/WallSlide")]
    public class WallSlide : StateData
    {
        public Vector3 maxFallVelocity;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.moveLeft = false;
            characterState.characterControl.moveRight = false;

            characterState.MomentumCalculator_Data.momentum = 0;
            characterState.characterControl.Air_Control.SetBool((int)AirControlBool.CAN_WALL_JUMP, false);

            characterState.VerticalVelocity_Data.maxWallSlideVelocity = maxFallVelocity;
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!characterState.characterControl.jump)
            {
                characterState.characterControl.Air_Control.SetBool((int)AirControlBool.CAN_WALL_JUMP, true);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.VerticalVelocity_Data.maxWallSlideVelocity = Vector3.zero;
        }
    }

}
