using UnityEngine.AI;
using UnityEngine;
using System;
using System.Drawing;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/AITriggerAttack")]
    public class AITriggerAttack : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (characterState.characterControl.turbo && 
                    characterState.characterControl.aiProgress.doFlyingKick &&
                       characterState.characterControl.aiProgress.TargetIsOnTheSamePlatform() && 
                           characterState.characterControl.aiProgress.AIDistanceToTarget() < 2f && //2 cause distance for straight is 1.5
                               !characterState.characterControl.aiProgress.TargetIsDead()) 
            {
                characterState.characterControl.attack = true;
            }
            else
            {
                characterState.characterControl.attack = false;
            }
        }
    }
}
