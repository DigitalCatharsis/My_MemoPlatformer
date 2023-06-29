using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/GravityPull")]
    public class GravityPull : StateData
    {
        public AnimationCurve gravity;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
            characterState.characterControl.gravityMultipliyer = gravity.Evaluate(stateInfo.normalizedTime);

        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.gravityMultipliyer = 0f;
        }
    }

}