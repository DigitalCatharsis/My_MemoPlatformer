using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum TransitionConditionType
{
    UP,
    DOWN,
    LEFT,
    RIGHT,
    ATTACK,
    JUMP,
    GRABBING_LEDGE,
    LEFT_OR_RIGHT,
    GROUNDED,
    MOVE_FORWARD,
    AIR,
    BLOCKED_BY_WALL,
    CAN_WALL_JUMP,
    NOT_GRABBING_LEDGE,
}

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/TransitionIndexer")]
    public class TransitionIndexer : StateData
    {
        public int Index;
        public List<TransitionConditionType> transitionConditions = new List<TransitionConditionType>();

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

            if (MakeTransition(characterState.characterControl))
            {
                animator.SetInteger(HashManager.Instance.dicMainParams[TransitionParameter.TransitionIndex], Index);
            }
        }



        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (animator.GetInteger(HashManager.Instance.dicMainParams[TransitionParameter.TransitionIndex]) == 0)
            {
                if (MakeTransition(characterState.characterControl))
                {
                    animator.SetInteger(HashManager.Instance.dicMainParams[TransitionParameter.TransitionIndex], Index);
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetInteger(HashManager.Instance.dicMainParams[TransitionParameter.TransitionIndex], 0);
        }

        private bool MakeTransition(CharacterControl control)
        {
            foreach (TransitionConditionType c in transitionConditions)
            {
                switch (c)
                {
                    case TransitionConditionType.UP:
                        {
                            if (!control.moveUp)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.DOWN:
                        {
                            if (!control.moveDown)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.LEFT:
                        {
                            if (!control.moveLeft)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.RIGHT:
                        {
                            if (!control.moveRight)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.ATTACK:
                        {
                            if (!control.animationProgress.attackTriggered)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.JUMP:
                        {
                            if (!control.jump)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.GRABBING_LEDGE:
                        {
                            if (!control.ledgeChecker.isGrabbongLedge)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.LEFT_OR_RIGHT:
                        {
                            if (!control.moveLeft && !control.moveRight)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.GROUNDED:
                        {
                            if (control.skinnedMeshAnimator.GetBool(HashManager.Instance.dicMainParams[TransitionParameter.Grounded]) == false)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.MOVE_FORWARD:
                        {
                            if (control.IsFacingForward())
                            {
                                if (!control.moveRight)
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                if (!control.moveLeft)
                                {
                                    return false;
                                }
                            }
                        }
                        break;
                    case TransitionConditionType.AIR:
                        {
                            if (!control.skinnedMeshAnimator.GetBool(HashManager.Instance.dicMainParams[TransitionParameter.Grounded]) == false)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.CAN_WALL_JUMP:
                        {
                            if (!control.animationProgress.canWallJump)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.NOT_GRABBING_LEDGE:
                        {
                            if (control.ledgeChecker.isGrabbongLedge)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.BLOCKED_BY_WALL:
                        {
                            if (control.animationProgress.blockingObj == null)
                            {
                                return false;
                            }
                            else  //if blockingObh != null it could be the character
                            {
                                if (CharacterManager.Instance.GetCharacter(control.animationProgress.blockingObj) != null)
                                {
                                    return false;
                                }
                            }
                        }
                        break;
                }
            }
            return true;
        }
    }
}