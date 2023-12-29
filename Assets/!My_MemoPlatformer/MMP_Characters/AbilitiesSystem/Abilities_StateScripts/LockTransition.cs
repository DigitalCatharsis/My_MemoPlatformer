using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/LockTransition")]
    public class LockTransition : CharacterAbility
    {
        public float unlockTime;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.animationProgress.lockTransition = true;
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (stateInfo.normalizedTime > unlockTime)
            {
                characterState.characterControl.animationProgress.lockTransition = false;
            }
            else
            {
                characterState.characterControl.animationProgress.lockTransition = true;
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }
    }

}
