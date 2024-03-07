using System;
using System.Collections;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AIProcessor : MonoBehaviour
    {
        private CharacterControl _control;
        private PathFindingAgent _pathFindingAgent;
        [SerializeField] private bool _aiHasReachedDestination = true;
        [SerializeField] private bool _finishedToClimb = true;

        private void Awake()
        {
            _control = GetComponentInParent<CharacterControl>();
        }

        private void OnEnable()
        {
            StartCoroutine(ProcessAI_Routine());
        }

        private IEnumerator ProcessAI_Routine()
        {
            yield return new WaitForEndOfFrame();

            _pathFindingAgent = _control.AICONTROLLER_DATA.pathfindingAgent;
            _pathFindingAgent.target = CharacterManager.Instance.GetPlayableCharacter().gameObject;

            Debug.Log("Started AI: " + _control.name);
            //SendPathfindingAgent();
            while (true)
            {
                Debug.Log("AIProcessot: new cycle");

                if (_control.PLAYER_ANIMATION_DATA.IsRunning(typeof(Landing)))
                {
                    _control.AICONTROLLER_DATA.aIBehavior.StopCharacter(); //get rid from MoveUp after jump
                }

                //if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToTarget() <= 1f)
                //{
                //    _control.AICONTROLLER_DATA.aIBehavior.ProcessAttack();
                //    _control.turbo = false;
                //    yield return new WaitForEndOfFrame();
                //    continue;
                //}

                if (_control.AICONTROLLER_DATA.aIConditions.CharacterIsGrounded(_control)
                    //&& _control.AICONTROLLER_DATA.aIConditions.CharacterIsGrounded(CharacterManager.Instance.GetPlayableCharacter())
                    //&& _aiHasReachedDestination
                    //&& _pathFindingAgent.hasFinishedPathfind
                    && !_control.PLAYER_ANIMATION_DATA.IsRunning(typeof(JumpPrep)))
                {
                    SendPathfindingAgent();
                }

                //Check for pathfind is finished
                if (!_pathFindingAgent.hasFinishedPathfind)
                {
                    Debug.Log("AOPROCESSOR: PA havent finished pathfind. Restarting Cycle");
                    yield return new WaitForSeconds(0.2f);
                    continue;
                }

                //Move
                Debug.Log("AIPROCESSOR: STARTING MOVING LOGIC");
                if (_control.AICONTROLLER_DATA.aIConditions.TargetIsOnTheSamePlatform())
                {
                    _control.AICONTROLLER_DATA.aIBehavior.MoveToTheStartSphere();

                    if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToStartSphere() <= 1f)
                    {
                        _aiHasReachedDestination = true;
                    }
                    else
                    {
                        _aiHasReachedDestination = false;
                    }
                }

                //if (!_control.AICONTROLLER_DATA.aIConditions.TargetIsOnTheSamePlatform())
                //{
                //    if (_control.AICONTROLLER_DATA.aIConditions.EndSphereIsHigherThanStartSphere())
                //    {
                //        if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToStartSphere() < 0.08f) //how close are we to the checkpoint    //����� ����� ������ ��� (����� ��� ������ ������ Start Point) ��-�� ������ �������� ��������� ��� �� ������������ ���� �����. ������ �� < 0.1f ��� ������
                //        {
                //            //_control.AICONTROLLER_DATA.aIBehavior.StopCharacter();
                //            _control.jump = true;
                //            _control.moveUp = true;

                //            //StopCoroutine(OnJumpingToPlatform_Routine());
                //            StartCoroutine(OnJumpingToPlatform_Routine());
                //        }
                //        else if (_control.AICONTROLLER_DATA.aIConditions.CharacterIsGrounded(_control))
                //        {
                //            _control.AICONTROLLER_DATA.aIBehavior.MoveToTheStartSphere();
                //        }
                //    }
                //}

                //Stop if we reach target
                Debug.Log("AIPROCESSOT: WE REACHED STARTSPHERE!");
                if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToEndSphere() < 1f)
                {
                    _control.AICONTROLLER_DATA.aIBehavior.StopCharacter();
                }

                //We should update spheres for keeping AI move
                Debug.Log("AIProcessor: UPDATING SPHERES POSITION!");
                if (_control.AICONTROLLER_DATA.aIConditions.TargetIsOnTheSamePlatform())
                {
                    _control.AICONTROLLER_DATA.aIBehavior.RepositionPESpheresDestination();
                }
                yield return new WaitForSeconds(0.3f);
            }
        }

        private IEnumerator OnJumpingToPlatform_Routine()
        {
            _finishedToClimb = false;
            while (_finishedToClimb == false)
            {          
                //Diffirence betwen character's top sphere (coliistion emulation) and End sphere of the pathfinding agent            
                var platformDistance = _pathFindingAgent.endSphere.transform.position
                    - _control.COLLISION_SPHERE_DATA.frontSpheres[0].transform.position;

                if (platformDistance.y > 0.5f)
                {
                    //TODO �������� �������� ������������ ��������� �����??

                    if (_pathFindingAgent.startSphere.transform.position.z <
                        _pathFindingAgent.endSphere.transform.position.z)
                    {
                        _control.moveRight = true;
                        _control.moveLeft = false;
                    }
                    else
                    {
                        _control.moveRight = false;
                        _control.moveLeft = true;
                    }
                }

                if (Math.Abs(platformDistance.z) < 0.1f)  //means it is on the same platform
                {
                    if (platformDistance.y < 0.3f)
                    {
                        _control.AICONTROLLER_DATA.aIBehavior.StopCharacter();
                        _aiHasReachedDestination = true;
                        _finishedToClimb = true;
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }
        }

        private void SendPathfindingAgent()
        {
            Debug.Log("Sending PE");
            if (_pathFindingAgent == null)
            {
                _pathFindingAgent = _control.AICONTROLLER_DATA.pathfindingAgent;
            }
            _pathFindingAgent.ProocedPathfindingAgent(_control);
        }
    }
}

