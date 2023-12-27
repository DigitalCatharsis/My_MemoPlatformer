using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/WallJump_Prep")]
    public class WallJump_Prep : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.moveLeft = false;
            characterState.characterControl.moveRight = false;
            characterState.MomentumCalculator_Data.momentum = 0f;

            characterState.characterControl.Rigid_Body.velocity = Vector3.zero;

            if (characterState.characterControl.PlayerRotation_Data.IsFacingForward())   //make the character turn 
            {
                characterState.characterControl.PlayerRotation_Data.FaceForward(false);
            }
            else
            {
                characterState.characterControl.PlayerRotation_Data.FaceForward(true);
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
