using System.Collections.Generic;
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
    CAN_WALLJUMP,
    NOT_GRABBING_LEDGE,
    NOT_BLOCKED_BY_WALL,
    MOVING_TO_BLOCKING_OBG,
    DOUBLETAP_UP,
    DOUBLETAP_DOWN,
    DOUBLETAP_LEFT,
    DOUBLETAP_RIGHT,
    TOUCHING_WEAPON,
    HOLDING_AXE,
    NOT_MOVING,
    RUN,
    NOT_RUN,
    BLOCKING,
    NOT_BLOCKING,
    ATTACK_ISBLOCKED,
}

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/TransitionIndexer")]
    public class TransitionIndexer : CharacterAbility
    {
        public int Index;
        public List<TransitionConditionType> transitionConditions = new List<TransitionConditionType>();

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

            if (MakeTransition(characterState.characterControl))
            {
                animator.SetInteger(HashManager.Instance.ArrMainParams[(int)MainParameterType.TransitionIndex], Index);
            }
        }



        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.PlayerJump_Data.checkWallBlock = StartCheckingWallBlock();

            if (animator.GetInteger(HashManager.Instance.ArrMainParams[(int)MainParameterType.TransitionIndex]) == 0)
            {
                if (MakeTransition(characterState.characterControl))
                {
                    animator.SetInteger(HashManager.Instance.ArrMainParams[(int)MainParameterType.TransitionIndex], Index);
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetInteger(HashManager.Instance.ArrMainParams[(int)MainParameterType.TransitionIndex], 0);
        }

        private bool StartCheckingWallBlock()
        {
            foreach (TransitionConditionType t in transitionConditions)
            {
                if (t == TransitionConditionType.BLOCKED_BY_WALL || t == TransitionConditionType.NOT_BLOCKED_BY_WALL)
                {
                    return true;
                }
            }

            return false;
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
                            if (!control.PlayerAttack_Data.attackTriggered)
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
                            if (!control.LedgeGrab_Data.isGrabbingLedge)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.NOT_GRABBING_LEDGE:
                        {
                            if (control.LedgeGrab_Data.isGrabbingLedge)
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
                            if (control.moveLeft && control.moveRight)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.GROUNDED:
                        {
                            if (control.skinnedMeshAnimator.GetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.Grounded]) == false)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.MOVE_FORWARD:
                        {
                            if (control.PlayerRotation_Data.IsFacingForward())
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
                            if (!control.skinnedMeshAnimator.GetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.Grounded]) == false)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.CAN_WALLJUMP:
                        {
                            var canWallJump = control.PlayerJump_Data.canWallJump;

                            if (!canWallJump)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.MOVING_TO_BLOCKING_OBG:
                        {
                            var objs = control.BlockingObj_Data.GetFrontBlockingObjList();

                            foreach (var o in objs) 
                            {
                                var dir = o.transform.position - control.transform.position;

                                if (dir.z > 0f && control.moveRight)
                                {
                                    return false;
                                }
                                if (dir.z < 0f && control.moveLeft)
                                {
                                    return false;
                                }
                            }
                        }
                        break;
                    case TransitionConditionType.BLOCKED_BY_WALL:
                        {
                            foreach (var oc in control.CollisionSpheres_Data.frontOverlapCheckers)
                            {
                                if (!oc.objIsOverlapping)
                                {
                                    return false;
                                }
                            }
                        }
                        break;
                    case TransitionConditionType.NOT_BLOCKED_BY_WALL:
                        {
                            bool allIsoverlapping = true;

                            foreach (var oc in control.CollisionSpheres_Data.frontOverlapCheckers)
                            {
                                if (!oc.objIsOverlapping)
                                {
                                    allIsoverlapping = false;
                                }
                            }

                            if (allIsoverlapping)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.DOUBLETAP_UP:
                        {
                            if (!control.subComponentProcessor.subcomponentsDictionary.ContainsKey(SubComponentType.MANUALINPUT))
                            {
                                return false;
                            }

                            if (!control.ManualInpu_Data.DoubleTapUp())
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.DOUBLETAP_DOWN:
                        {
                            if (!control.subComponentProcessor.subcomponentsDictionary.ContainsKey(SubComponentType.MANUALINPUT))
                            {
                                return false;
                            }

                            if (!control.ManualInpu_Data.DoubleTapDown())
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.TOUCHING_WEAPON:
                        {
                            if (control.animationProgress.collidingWeapons.Count == 0)
                            {
                                if (control.animationProgress.HoldingWeapon == null)
                                {
                                    return false;
                                }
                            }
                        }
                        break;
                    case TransitionConditionType.HOLDING_AXE:
                        {
                            if (control.animationProgress.HoldingWeapon == null)
                            {
                                return false;
                            }

                            if (!control.animationProgress.HoldingWeapon.name.Contains("Blade_Knife"))
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.NOT_MOVING:
                        {
                            if (control.moveLeft || control.moveRight)
                            {
                                if (!(control.moveLeft && control.moveRight))
                                {
                                    return false;
                                }
                            }
                        }
                        break;
                    case TransitionConditionType.RUN:
                        {
                            if (!control.moveLeft && !control.moveRight)
                            {
                                return false;
                            }
                            if (control.moveLeft && control.moveRight)
                            {
                                return false;
                            }
                            if (!control.turbo)
                            {
                                return false;
                            }
                        }

                        break;
                    case TransitionConditionType.NOT_RUN:
                        {
                            if (control.turbo)
                            {
                                if (control.moveLeft || control.moveRight)
                                {
                                    if (!(control.moveLeft && control.moveRight))
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                        break;
                    case TransitionConditionType.BLOCKING:
                        {
                            if (!control.block)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.NOT_BLOCKING:
                        {
                            if (control.block)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.ATTACK_ISBLOCKED:
                        {
                            if (control.DamageDetector_Data.blockedAttack == null)
                            {
                                return false;
                            }
                        }
                        break;
                }
            }
            return true;
        }
    }
}