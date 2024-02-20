using UnityEngine;

namespace My_MemoPlatformer
{
    public class AIConditions : MonoBehaviour
    {
        private AIController _aiController;

        private void Start()
        {
            _aiController = GetComponent<AIController>();
        }
        private bool IsAttacking()
        {
            var info = _aiController.Control.PLAYER_ANIMATION_DATA.animator.GetCurrentAnimatorStateInfo(0);

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
            if ((_aiController.pathfindingAgent.transform.position - _aiController.Control.transform.position).z > 0f)
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
            if ((_aiController.pathfindingAgent.target.transform.position - _aiController.Control.transform.position).z > 0f)
            {
                if (_aiController.Control.ROTATION_DATA.IsFacingForward())
                {
                    return true;
                }
            }
            else
            {
                if (!_aiController.Control.ROTATION_DATA.IsFacingForward())
                {
                    return true;
                }
            }

            return false;
        }
        private bool TargetIsDead()
        {
            if (CharacterManager.Instance.GetCharacter(_aiController.pathfindingAgent.target).DAMAGE_DATA.IsDead())
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool EndSphereIsHigher()
        {
            if (EndSphereIsStraight())
            {
                return false;
            }

            if (_aiController.pathfindingAgent.endSphere.transform.position.y - _aiController.pathfindingAgent.startSphere.transform.position.y > 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool EndSphereIsLower()
        {
            if (EndSphereIsStraight())
            {
                return false;
            }

            if (_aiController.pathfindingAgent.endSphere.transform.position.y - _aiController.pathfindingAgent.startSphere.transform.position.y > 0f)
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
            if (Mathf.Abs(_aiController.pathfindingAgent.endSphere.transform.position.y - _aiController.pathfindingAgent.startSphere.transform.position.y) > 0.01f)
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
            var target = CharacterManager.Instance.GetCharacter(_aiController.pathfindingAgent.target);
            if (target.GROUND_DATA.ground == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool TargetIsOnTheSamePlatform()
        {
            var target = CharacterManager.Instance.GetCharacter(_aiController.pathfindingAgent.target);

            if (target.GROUND_DATA.ground == _aiController.Control.GROUND_DATA.ground)
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