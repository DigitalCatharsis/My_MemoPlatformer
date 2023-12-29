using UnityEngine;

namespace My_MemoPlatformer
{
    namespace Roundbeargames
    {
        [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/LockTurn")]
        public class LockTurn : CharacterAbility
        {
            [Range(0f, 1f)]
            public float lockTiming;

            [Range(0f, 1f)]
            public float unlockTiming;

            public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
            {

            }

            public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
            {
                if (stateInfo.normalizedTime >= lockTiming && !characterState.Rotation_Data.lockTurn)
                {
                    characterState.Rotation_Data.lockTurn = true;
                    characterState.Rotation_Data.unlockTiming = unlockTiming;
                }
            }

            public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
            {

            }
        }
    }
}