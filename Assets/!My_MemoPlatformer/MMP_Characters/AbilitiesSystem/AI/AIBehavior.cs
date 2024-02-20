using UnityEngine;

namespace My_MemoPlatformer
{
    public class AIBehavior : MonoBehaviour
    {
        private AIController _controller;
        private Vector3 _targetDir;

        private void Start()
        {
            _controller = GetComponent<AIController>();
        }



        public void StartAi()
        {
            _controller.aiAnimator.Play(HashManager.Instance.arrAIStateNames[(int)AI_State_Name.SendPathfindingAgent], 0);
        }

        private void RepositionDestination()
        {
            _controller.pathfindingAgent.startSphere.transform.position = _controller.pathfindingAgent.target.transform.position;
            _controller.pathfindingAgent.endSphere.transform.position = _controller.pathfindingAgent.target.transform.position;
        }
        private void ProcessAttack()
        {
            _controller._aIAttacks.Attack();
        }

        private void WalkStraightToTheStartSphere()
        {
            _targetDir = _controller.pathfindingAgent.startSphere.transform.position - _controller.Control.transform.position;

            if (_targetDir.z > 0f)
            {
                _controller.Control.moveLeft = false;
                _controller.Control.moveRight = true;
            }
            else
            {
                _controller.Control.moveLeft = true;
                _controller.Control.moveRight = false;
            }
        }
        private void WalkStraightToTheEndSphere()
        {
            _targetDir = _controller.pathfindingAgent.endSphere.transform.position - _controller.Control.transform.position;

            if (_targetDir.z > 0f)
            {
                _controller.Control.moveLeft = false;
                _controller.Control.moveRight = true;
            }
            else
            {
                _controller.Control.moveLeft = true;
                _controller.Control.moveRight = false;
            }
        }

        private bool RestartWalk()
        {
            if (_controller._aiLogistic.AIDistanceToEndSphere() < 1f)
            {
                if (_controller._aiLogistic.TargetDistanceToEndSphere() > 0.5f)
                {
                    if (_controller._aIConditions.TargetIsGrounded())
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}