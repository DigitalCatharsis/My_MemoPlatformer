using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AIConditions : MonoBehaviour
    {
        private CharacterControl _control;

        private void Start()
        {
            _control = GetComponentInParent<CharacterControl>();
        }
        public bool IsRunningCondition()
        {
            //straight
            if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToStartSphere() > 1.5f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //public bool IsInAttackingAnimation()
        //{
        //    var info = _control.AICONTROLLER_DATA.aiAnimator.GetCurrentAnimatorStateInfo(0);

        //    if (info.shortNameHash == HashManager.Instance.arrAIStateNames[(int)AI_State_Name.AI_Attack])
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        public bool TargetIsOnRightSide()
        {
            if ((_control.AICONTROLLER_DATA.pathfindingAgent.target.transform.position - _control.transform.position).z > 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsFacingTarget()
        {
            if ((_control.AICONTROLLER_DATA.pathfindingAgent.target.transform.position - _control.transform.position).z > 0f)
            {
                if (_control.ROTATION_DATA.IsFacingForward())
                {
                    return true;
                }
            }
            else
            {
                if (!_control.ROTATION_DATA.IsFacingForward())
                {
                    return true;
                }
            }

            return false;
        }
        public bool TargetIsDead()
        {
            if (CharacterManager.Instance.GetCharacter(_control.AICONTROLLER_DATA.pathfindingAgent.target).DAMAGE_DATA.IsDead())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool EndSphereIsHigherThanStartSphere()
        {
            if (EndSphereIsStraightWithStart())
            {
                return false;
            }

            if (_control.AICONTROLLER_DATA.pathfindingAgent.endSphere.transform.position.y - _control.AICONTROLLER_DATA.pathfindingAgent.startSphere.transform.position.y > 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //public bool EndSphereIsLowerThanStartSphere()
        //{
        //    if (EndSphereIsStraightWithStart())
        //    {
        //        return false;
        //    }

        //    if (_control.AICONTROLLER_DATA.pathfindingAgent.endSphere.transform.position.y - _control.AICONTROLLER_DATA.pathfindingAgent.startSphere.transform.position.y > 0f)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}

        private bool EndSphereIsStraightWithStart()
        {
            if (Mathf.Abs(_control.AICONTROLLER_DATA.pathfindingAgent.endSphere.transform.position.y - _control.AICONTROLLER_DATA.pathfindingAgent.startSphere.transform.position.y) > 0.01f)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool CharacterIsGrounded(CharacterControl character)
        {
            if (character.GROUND_DATA.ground == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool TargetIsOnTheSamePlatform()
        {
            var target = CharacterManager.Instance.GetCharacter(_control.AICONTROLLER_DATA.pathfindingAgent.target);

            if (target.GROUND_DATA.ground == null && _control.GROUND_DATA.ground == null)
            {
                return false;
            }

            if (target.GROUND_DATA.ground == _control.GROUND_DATA.ground)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool FrontBlockedByCharacter()
        {
            //If someone blocking - restart PA
            SetFrontBlockedCharacter();
            if (_control.AICONTROLLER_DATA.blockingCharacter != null)
            {
                if (_control.GROUND_DATA.ground != null)
                {
                    if (!_control.PLAYER_ANIMATION_DATA.IsRunning(typeof(Jump)) &&
                        !_control.PLAYER_ANIMATION_DATA.IsRunning(typeof(JumpPrep)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        private void SetFrontBlockedCharacter()
        {
            if (_control.BLOCKING_OBJ_DATA.frontBlockingDictionaryCount == 0)
            {
                _control.AICONTROLLER_DATA.blockingCharacter = null;
            }
            else
            {
                List<GameObject> objs = _control.BLOCKING_OBJ_DATA.GetFrontBlockingCharactersList();

                foreach (GameObject o in objs)
                {
                    CharacterControl blockingChar = CharacterManager.Instance.GetCharacter(o);

                    if (blockingChar != null)
                    {
                        _control.AICONTROLLER_DATA.blockingCharacter = blockingChar;
                    }
                    else
                    {
                        _control.AICONTROLLER_DATA.blockingCharacter = null;
                    }
                }
            }
        }
        public bool IsStartSphereOnSameY()
        {
            var height = _control.AICONTROLLER_DATA.aiLogistic.GetStartSphereABSHeight();
            if (height > 0.1f)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //public bool AIIsOnGround(CharacterControl control)
        //{
        //    if (control.PLAYER_ANIMATION_DATA.IsRunning(typeof(MoveUp)))
        //    {
        //        return false;
        //    }

        //    if (control.rigidBody.useGravity)
        //    {
        //        if (control.skinnedMeshAnimator.GetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]))
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}
    }
}