using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/Turn180")]
    public class Turn180 : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {        
            if (characterState.Rotation_Data.IsFacingForward())
            {
                characterState.Rotation_Data.FaceForward(false);
            }
            else
            {
                characterState.Rotation_Data.FaceForward(true);
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
