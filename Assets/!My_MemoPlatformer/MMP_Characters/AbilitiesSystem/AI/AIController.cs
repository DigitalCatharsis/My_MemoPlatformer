using UnityEngine;

namespace My_MemoPlatformer
{
    public class AIController : MonoBehaviour
    {
        private Vector3 _targetDir = new Vector3();
        private CharacterControl _control;
        private Animator _animatorController;

        public Animator ANIMATOR => _animatorController;

        private void Awake()
        {
            _control = this.gameObject.GetComponentInParent<CharacterControl>();

            _animatorController = this.gameObject.GetComponentInChildren<Animator>();

            var arr = _animatorController.GetBehaviours<CharacterState>();

            foreach (var aiState in arr)
            {
                aiState.characterControl = _control;
            }
        }

        public void InitializeAI()
        {
            ANIMATOR.Play(HashManager.Instance.arrAIStateNames[(int)AI_State_Name.SendPathfindingAgent], 0);
        }

        public void WalkStraightToTheStartSphere()
        {
            _targetDir = _control.aiProgress.pathfindingAgent.startSphere.transform.position - _control.transform.position;

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
            _targetDir = _control.aiProgress.pathfindingAgent.endSphere.transform.position - _control.transform.position;

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

        public bool RestartWalk()
        {
            if (_control.aiProgress.AIDistanceToEndSphere() < 1f)
            {
                if (_control.aiProgress.TargetDistanceToEndSphere() > 0.5f)
                {
                    if (_control.aiProgress.TargetIsGrounded())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool IsAttacking()
        {
            var info = _control.aiController.ANIMATOR.GetCurrentAnimatorStateInfo(0);

            if (info.shortNameHash == HashManager.Instance.arrAIStateNames[(int)AI_State_Name.AI_Attack])
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