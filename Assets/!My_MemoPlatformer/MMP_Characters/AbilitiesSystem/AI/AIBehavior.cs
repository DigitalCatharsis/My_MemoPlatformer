using System.Collections;
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
            _control.AICONTROLLER_DATA.aiStatus = Ai_Status.Starting_AI.ToString();
            _control.AICONTROLLER_DATA.aiAnimator.Play(HashManager.Instance.arrAIStateNames[(int)AI_State_Name.SendPathfindingAgent], 0);
        }

        public void RepositionPESpheresDestination()
        {
            _control.AICONTROLLER_DATA.aiStatus = Ai_Status.Repositioning_Destination.ToString();
            //TODO: часто тут застревает
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

        public void TriggerAttackState(CharacterControl control)
        {
            _control.AICONTROLLER_DATA.aiStatus = Ai_Status.Triggering_AI_State.ToString();
            control.AICONTROLLER_DATA.aiAnimator.Play(HashManager.Instance.arrAIStateNames[(int)AI_State_Name.AI_Attack], 0);
        }

        public void MoveToTheStartSphere()
        {
            _control.AICONTROLLER_DATA.aiStatus = Ai_Status.Walking_To_StartSphere.ToString();
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
        public void WalkStraightToTheEndSphere()
        {
            _control.AICONTROLLER_DATA.aiStatus = Ai_Status.Walking_To_EndSphere.ToString();

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
            _control.moveUp = false;
            _control.moveRight = false;
            _control.moveLeft = false;
            _control.attack = false;
            _control.turbo = false;
            _control.jump = false;
        }
    }
}