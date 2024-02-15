using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/WeaponPutDown")]
    public class WeaponPutDown : CharacterAbility
    {
        public float putDownTiming;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (stateInfo.normalizedTime > putDownTiming)
            {
                if (characterState.characterControl.ATTACK_DATA.holdingWeapon != null)
                {
                    characterState.characterControl.ATTACK_DATA.holdingWeapon.DropWeapon();
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
    }
}
