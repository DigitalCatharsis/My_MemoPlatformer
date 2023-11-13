using UnityEditor.Rendering.Universal;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/OffsetOnLedge")]
    public class OffsetOnLedge : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!characterState.characterControl.Rigid_Body.useGravity)
            {
                characterState.characterControl.Rigid_Body.MovePosition(characterState.characterControl.ledgeChecker.grabbedLedge.transform.position
                    + characterState.characterControl.ledgeChecker.grabbedLedge.offset);           
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }
    }

}
