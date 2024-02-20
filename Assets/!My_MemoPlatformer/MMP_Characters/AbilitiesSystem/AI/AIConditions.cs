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
        public bool IsAttacking()
        {
            var info = _control.PLAYER_ANIMATION_DATA.animator.GetCurrentAnimatorStateInfo(0);

            if (info.shortNameHash == HashManager.Instance.arrAIStateNames[(int)AI_State_Name.AI_Attack])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool TargetIsOnRightSide()
        {
            if ((_control.AICONTROLLER_DATA.pathfindingAgent.transform.position - _control.transform.position).z > 0f)
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
        public bool EndSphereIsHigher()
        {
            if (EndSphereIsStraight())
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
        public bool EndSphereIsLower()
        {
            if (EndSphereIsStraight())
            {
                return false;
            }

            if (_control.AICONTROLLER_DATA.pathfindingAgent.endSphere.transform.position.y - _control.AICONTROLLER_DATA.pathfindingAgent.startSphere.transform.position.y > 0f)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool EndSphereIsStraight()
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
        public bool TargetIsGrounded()
        {
            var target = CharacterManager.Instance.GetCharacter(_control.AICONTROLLER_DATA.pathfindingAgent.target);
            if (target.GROUND_DATA.ground == null)
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

            if (target.GROUND_DATA.ground == _control.GROUND_DATA.ground)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}