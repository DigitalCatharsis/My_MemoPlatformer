using My_MemoPlatformer;
using System.Collections.Generic;
using UnityEngine;

namespace Roundbeargames
{
    public static class TransitionConditionChecker
    {
        public static bool MakeTransition(CharacterControl control, List<TransitionConditionType> transitionConditions)
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
                            if (!control.ATTACK_DATA.attackTriggered)
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
                            if (!control.LEDGE_GRAB_DATA.isGrabbingLedge)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.NOT_GRABBING_LEDGE:
                        {
                            if (control.LEDGE_GRAB_DATA.isGrabbingLedge)
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
                            if (control.skinnedMeshAnimator.
                                GetBool(HashManager.Instance.arrMainParams[
                                    (int)MainParameterType.Grounded]) == false)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.MOVE_FORWARD:
                        {
                            if (control.ROTATION_DATA.IsFacingForward())
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
                    case TransitionConditionType.NOT_GROUNDED:
                        {
                            if (!control.skinnedMeshAnimator.
                                GetBool(HashManager.Instance.arrMainParams[
                                    (int)MainParameterType.Grounded]) == false)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.BLOCKED_BY_WALL:
                        {
                            for (int i = 0; i < control.COLLISION_SPHERE_DATA.frontOverlapCheckers.Length; i++)
                            {
                                if (!control.COLLISION_SPHERE_DATA.frontOverlapCheckers[i].objIsOverlapping)
                                {
                                    return false;
                                }
                            }

                            //foreach (OverlapChecker oc in control.COLLISION_SPHERE_DATA.FrontOverlapCheckers)
                            //{
                            //    if (!oc.ObjIsOverlapping)
                            //    {
                            //        return false;
                            //    }
                            //}
                        }
                        break;
                    case TransitionConditionType.NOT_BLOCKED_BY_WALL:
                        {
                            bool AllIsOverlapping = true;

                            for (int i = 0; i < control.COLLISION_SPHERE_DATA.frontOverlapCheckers.Length; i++)
                            {
                                if (!control.COLLISION_SPHERE_DATA.frontOverlapCheckers[i].objIsOverlapping)
                                {
                                    AllIsOverlapping = false;
                                }
                            }

                            //foreach (OverlapChecker oc in control.COLLISION_SPHERE_DATA.FrontOverlapCheckers)
                            //{
                            //    if (!oc.ObjIsOverlapping)
                            //    {
                            //        AllIsOverlapping = false;
                            //    }
                            //}

                            if (AllIsOverlapping)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.CAN_WALLJUMP:
                        {
                            if (!control.JUMP_DATA.canWallJump)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.MOVING_TO_BLOCKING_OBJ:
                        {
                            List<GameObject> objs = control.BLOCKING_OBJ_DATA.GetFrontBlockingObjList();

                            foreach (GameObject o in objs)
                            {
                                Vector3 dir = o.transform.position - control.transform.position;

                                if (dir.z > 0f && !control.moveRight)
                                {
                                    return false;
                                }

                                if (dir.z < 0f && !control.moveLeft)
                                {
                                    return false;
                                }
                            }
                        }
                        break;
                    case TransitionConditionType.DOUBLE_TAP_UP:
                        {
                            if (control.subComponentProcessor.arrSubComponents[(int)SubComponentType.MANUAL_INPUT] == null)
                            {
                                return false;
                            }
                            //if (!control.subComponentProcessor.ComponentsDic.ContainsKey(SubComponentType.MANUALINPUT))
                            //{
                            //    return false;
                            //}

                            if (!control.MANUAL_INPUT_DATA.DoubleTapUp())
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.DOUBLE_TAP_DOWN:
                        {
                            if (control.subComponentProcessor.arrSubComponents[(int)SubComponentType.MANUAL_INPUT] == null)
                            {
                                return false;
                            }
                            //if (!control.subComponentProcessor.ComponentsDic.ContainsKey(SubComponentType.MANUALINPUT))
                            //{
                            //    return false;
                            //}

                            if (!control.MANUAL_INPUT_DATA.DoubleTapDown())
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.DOUBLE_TAP_LEFT:
                        {
                            return false;
                        }
                        break;
                    case TransitionConditionType.DOUBLE_TAP_RIGHT:
                        {
                            return false;
                        }
                        break;
                    case TransitionConditionType.TOUCHING_WEAPON:
                        {
                            if (control.animationProgress.collidingWeapons.Count == 0)
                            {
                                if (control.animationProgress.holdingWeapon == null)
                                {
                                    return false;
                                }
                            }
                        }
                        break;
                    case TransitionConditionType.HOLDING_AXE:
                        {
                            if (control.animationProgress.holdingWeapon == null)
                            {
                                return false;
                            }

                            if (!control.animationProgress.holdingWeapon.name.Contains("Axe"))
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
                            if (!control.turbo)
                            {
                                return false;
                            }

                            if (control.moveLeft && control.moveRight)
                            {
                                return false;
                            }

                            if (!control.moveLeft && !control.moveRight)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.NOT_RUNNING:
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
                    case TransitionConditionType.ATTACK_IS_BLOCKED:
                        {
                            if (control.DAMAGE_DATA.blockedAttack == null)
                            {
                                return false;
                            }
                        }
                        break;
                    case TransitionConditionType.NOT_TURBO:
                        {
                            if (control.turbo)
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