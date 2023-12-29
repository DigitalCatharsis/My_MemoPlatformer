using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/WeaponThrow")]
    public class WeaponThrow : CharacterAbility
    {
        public float throwTiming;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (stateInfo.normalizedTime > throwTiming)
            {
                if (characterState.characterControl.animationProgress.holdingWeapon != null)
                {
                    characterState.characterControl.animationProgress.holdingWeapon.ThrowWeapon();
                    //characterState.characterControl.animationProgress.HoldingWeapon.DropWeapon();
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
    }
}
