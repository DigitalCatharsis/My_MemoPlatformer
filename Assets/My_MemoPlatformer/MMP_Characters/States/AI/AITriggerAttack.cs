using UnityEngine;
using System.Collections.Generic;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/AITriggerAttack")]
    public class AITriggerAttack : StateData
    {
        delegate void GroundAttack(CharacterControl control);
        private List<GroundAttack> _listGroundAttacks = new List<GroundAttack>();
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (_listGroundAttacks.Count == 0)
            {
                _listGroundAttacks.Add(NormalGroundAttack);
                _listGroundAttacks.Add(ForwardGroundAttack);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (characterState.characterControl.aiProgress.TargetIsDead())
            {
                characterState.characterControl.attack = false;
                return;
            }
            if (characterState.characterControl.turbo && characterState.characterControl.aiProgress.AIDistanceToTarget() < 2f) //2 cause distance for straight is 1.5)
            {
                FlyingKick(characterState.characterControl);
            }
            else if (!characterState.characterControl.turbo && characterState.characterControl.aiProgress.AIDistanceToTarget() < 1f)
            {
                _listGroundAttacks[Random.Range(0, _listGroundAttacks.Count)](characterState.characterControl);
            }
            else
            {
                characterState.characterControl.attack = false;
            }
        }

        public void NormalGroundAttack(CharacterControl control)
        {
            if (control.aiProgress.TargetIsOnTheSamePlatform())
            {
                control.moveRight = false;
                control.moveLeft = false;

                if (control.aiProgress.IsFacingTarget() && !control.animationProgress.IsRunning(typeof(MoveForward)))
                {
                    control.attack = true;
                }
                else
                {
                    control.attack = false;
                }

            }
        }
        public void ForwardGroundAttack(CharacterControl control)
        {
            if (control.aiProgress.TargetIsOnTheSamePlatform())
            {
                if (control.aiProgress.TargetIsOnRightSide())
                {
                    control.moveRight = true;
                    control.moveLeft = false;

                    if (control.aiProgress.IsFacingTarget() && control.animationProgress.IsRunning(typeof(MoveForward)))
                    {
                        control.attack = true;
                    }
                }
                else
                {
                    control.moveRight = false;
                    control.moveLeft = true;

                    if (control.aiProgress.IsFacingTarget() && control.animationProgress.IsRunning(typeof(MoveForward)))
                    {
                        control.attack = true;
                    }
                }
            }
            else
            {
                control.attack = false;
            }
        }

        public void FlyingKick(CharacterControl control)
        {
            if (control.aiProgress.doFlyingKick &&
                    control.aiProgress.TargetIsOnTheSamePlatform())

            {
                control.attack = true;
            }
            else
            {
                control.attack = false;
            }
        }
    }
}
