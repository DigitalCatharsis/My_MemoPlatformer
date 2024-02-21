using UnityEngine;
using System.Collections.Generic;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/AITriggerAttack")]
    public class AITriggerAttack : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (characterState.characterControl.AICONTROLLER_DATA.aIConditions.TargetIsDead())
            {
                characterState.characterControl.attack = false;
            }
            else
            {
                if (characterState.characterControl.AICONTROLLER_DATA.aiLogistic.AIDistanceToTarget() < 8f)
                {
                    if (!FlyingKick(characterState.characterControl))
                    {
                        if (characterState.characterControl.AICONTROLLER_DATA.aiLogistic.AIDistanceToTarget() < 2f)
                        {
                            TriggerAttack(characterState.characterControl);
                        }
                    }
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }

        private bool FlyingKick(CharacterControl control)
        {
            if (control.AICONTROLLER_DATA.doFlyingKick && control.AICONTROLLER_DATA.aIConditions.TargetIsOnTheSamePlatform())
            {
                control.attack = true;
                return true;
            }
            else
            {
                control.attack = false;
                return false;
            }
        }

        private void TriggerAttack(CharacterControl control)
        {
            control.PLAYER_ANIMATION_DATA.animator.Play(HashManager.Instance.arrAIStateNames[(int)AI_State_Name.AI_Attack], 0);
        }
    }
}
