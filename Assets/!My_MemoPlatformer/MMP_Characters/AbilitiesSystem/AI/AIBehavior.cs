using UnityEngine;

namespace My_MemoPlatformer
{
    public class AIBehavior : MonoBehaviour
    {
        private Vector3 _targetDir;
        private CharacterControl _control;

        private void Start()
        {
            _control = GetComponentInParent<CharacterControl>();
        }

        public void StartAi()
        {
            _control.AICONTROLLER_DATA.aiAnimator.Play(HashManager.Instance.arrAIStateNames[(int)AI_State_Name.SendPathfindingAgent], 0);
        }

        public void RepositionDestination()
        {
            _control.AICONTROLLER_DATA.pathfindingAgent.startSphere.transform.position = _control.AICONTROLLER_DATA.pathfindingAgent.target.transform.position;
            _control.AICONTROLLER_DATA.pathfindingAgent.endSphere.transform.position = _control.AICONTROLLER_DATA.pathfindingAgent.target.transform.position;
        }
        public void ProcessAttack()
        {
            _control.AICONTROLLER_DATA.aIAttacks.Attack();
        }

        public void WalkStraightToTheStartSphere()
        {
            _targetDir = _control.AICONTROLLER_DATA.pathfindingAgent.startSphere.transform.position - _control.transform.position;

            if (_targetDir.z > 0f)
            {
                _control.moveLeft = false;
                _control.moveRight = true;
            }
            else
            {
                _control.moveLeft = true;
                _control.moveRight = false;
            }
        }
        public void WalkStraightToTheEndSphere()
        {
            _targetDir = _control.AICONTROLLER_DATA.pathfindingAgent.endSphere.transform.position - _control.transform.position;

            if (_targetDir.z > 0f)
            {
                _control.moveLeft = false;
                _control.moveRight = true;
            }
            else
            {
                _control.moveLeft = true;
                _control.moveRight = false;
            }
        }

        public bool IsRestartWalkCondition()
        {
            if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToEndSphere() < 1f)
            {
                if (_control.AICONTROLLER_DATA.aiLogistic.TargetDistanceToEndSphere() > 0.5f)
                {
                    if (_control.AICONTROLLER_DATA.aIConditions.TargetIsGrounded())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}