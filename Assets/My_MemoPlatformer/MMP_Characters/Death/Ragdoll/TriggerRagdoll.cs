using My_MemoPlatformer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/Death/TriggerRagdoll")]
    public class TriggerRagdoll : StateData
    {
        public float triggerTiming;
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }



        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);

            if (stateInfo.normalizedTime >= triggerTiming)
            {
                if (!control.animationProgress.ragdollTriggered)
                {
                    control.TurnOnRagdoll();
                    control.animationProgress.ragdollTriggered = true;
                }

            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);
            control.animationProgress.ragdollTriggered = false;
        }
    }
}