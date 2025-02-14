using System.Collections;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AIBehaviors : MonoBehaviour
    {
        private Vector3 _targetDir;
        private CharacterControl _control;

        private void Start()
        {
            _control = GetComponentInParent<CharacterControl>();
        }

        public void TryProcessFlyingKick()
        {
            _control.AICONTROLLER_DATA.aIAttacks.SetRandomFlyingKick();

            if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToTarget() < 8f
                && _control.AICONTROLLER_DATA.aiLogistic.AIDistanceToTarget() > 3f
                && _control.AICONTROLLER_DATA.aIConditions.IsFacingTarget())
            {
                _control.AICONTROLLER_DATA.aIAttacks.ProceedFlyingKick(_control);
            }
        }

        public void ResetPASpheresPosition()
        {
            _control.AICONTROLLER_DATA.aiStatus = Ai_Status.ResetingPASpheresPosition.ToString();
            _control.AICONTROLLER_DATA.pathfindingAgent.startSphere.transform.position = _control.AICONTROLLER_DATA.pathfindingAgent.target.transform.position;
            _control.AICONTROLLER_DATA.pathfindingAgent.endSphere.transform.position = _control.AICONTROLLER_DATA.pathfindingAgent.target.transform.position;
        }
        public void ProcessAttack()
        {
            _control.AICONTROLLER_DATA.aiStatus = Ai_Status.Attacking.ToString();
            _control.AICONTROLLER_DATA.listGroundAttacks[RandomizeNextAttack()](_control);
        }
        private int RandomizeNextAttack()
        {
            return Random.Range(0, _control.AICONTROLLER_DATA.listGroundAttacks.Count);
        }

        public void MoveToTheStartSphere()
        {
            _control.AICONTROLLER_DATA.aiStatus = Ai_Status.Moving_To_StartSphere.ToString();
            _targetDir = _control.AICONTROLLER_DATA.pathfindingAgent.startSphere.transform.position - _control.transform.position;

            _control.turbo = _control.AICONTROLLER_DATA.aIConditions.IsRunningCondition();

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
        public void MoveToTheEndSphere()
        {
            _control.AICONTROLLER_DATA.aiStatus = Ai_Status.Moving_To_EndSphere.ToString();

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

        public void StopCharacter()
        {
            _control.AICONTROLLER_DATA.aiStatus = Ai_Status.StopingCharacter.ToString();
            _control.jump = false;
            _control.moveUp = false;
            _control.moveRight = false;
            _control.moveLeft = false;
            _control.turbo = false;
        }
    }
}