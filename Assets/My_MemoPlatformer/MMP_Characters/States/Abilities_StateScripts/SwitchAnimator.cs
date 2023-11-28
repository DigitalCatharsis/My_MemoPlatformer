using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/SwitchAnimator")]
    public class SwitchAnimator : StateData
    {
        public float switchTiming;
        public RuntimeAnimatorController targetAnimator;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.Rigid_Body.useGravity = true;
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (stateInfo.normalizedTime >= switchTiming)
            {
                characterState.characterControl.skinnedMeshAnimator.runtimeAnimatorController = targetAnimator;
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
    }
}
