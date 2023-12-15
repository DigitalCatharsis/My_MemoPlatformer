using My_MemoPlatformer.Datasets;
using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/WallJump_Prep")]
    public class WallJump_Prep : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.moveLeft = false;
            characterState.characterControl.moveRight = false;
            characterState.characterControl.Air_Control.SetFloat((int)AirControlFloat.AIR_MOMENTUM, 0.0f);

            characterState.characterControl.Rigid_Body.velocity = Vector3.zero;

            if (characterState.characterControl.IsFacingForward())   //make the character turn 
            {
                characterState.characterControl.FaceForward(false);
            }
            else
            {
                characterState.characterControl.FaceForward(true);
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
    }

}
