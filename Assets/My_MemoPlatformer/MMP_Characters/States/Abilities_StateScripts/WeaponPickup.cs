using System.Net;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/WeaponPickup")]
    public class WeaponPickup : StateData
    {
        public float pickupTiming;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (stateInfo.normalizedTime > pickupTiming)
            {
                if (characterState.characterControl.animationProgress.HoldingWeapon == null)
                {
                    var weapon = characterState.characterControl.animationProgress.GetTouchingWeapon();
                    characterState.characterControl.animationProgress.HoldingWeapon = weapon;

                    weapon.transform.parent = characterState.characterControl.rightHand_Attack.transform;
                    weapon.transform.localPosition = weapon.customPosition;
                    weapon.transform.localRotation = Quaternion.Euler(weapon.customRotation);

                    weapon.control = characterState.characterControl;
                    weapon.triggerDetector.control = characterState.characterControl;

                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
    }

}