using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AIProcessor : MonoBehaviour
    {
        private CharacterControl _control;
        private PathFindingAgent _pathFindingAgent;
        [SerializeField] private bool _finishedToClimb = true;

        private const float _minimumDistanceToStartSphereForJump = 0.1f;

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
            ResultWrapper<bool> finishedMoveRoutine = new();

            _control.AICONTROLLER_DATA.aiStatus = Ai_Status.StartingAiProcessor.ToString();
            Debug.Log("Started AI: " + _control.name);
            yield return null; //get time to find PA

            _pathFindingAgent = _control.AICONTROLLER_DATA.pathfindingAgent;
            _pathFindingAgent.target = CharacterManager.Instance.GetPlayableCharacter().gameObject;

            while (true)
            {
                _control.AICONTROLLER_DATA.aiStatus = Ai_Status.RestartingProcessorCycle.ToString();

                finishedMoveRoutine.Result = true;
                Debug.Log("AIProcessot: new cycle");
                //Init PA
                if (_pathFindingAgent == null)
                {
                    _pathFindingAgent = _control.AICONTROLLER_DATA.pathfindingAgent;
                }

                //Rest if landing
                if (_control.PLAYER_ANIMATION_DATA.IsRunning(typeof(Landing)))
                {
                    _control.AICONTROLLER_DATA.aIBehavior.StopCharacter(); //get rid from MoveUp after jump
                    yield return null;
                    continue;
                }

                //Attack?
                _control.AICONTROLLER_DATA.aIAttacks.SetRandomFlyingKick();

                if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToTarget() < 8f
                    && _control.AICONTROLLER_DATA.aiLogistic.AIDistanceToTarget() > 3f
                    && _control.AICONTROLLER_DATA.aIConditions.IsFacingTarget())
                {
                    _control.AICONTROLLER_DATA.aIAttacks.ProceedFlyingKick(_control);
                }

                if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToTarget() <= 1f)
                {
                    _control.AICONTROLLER_DATA.aIBehavior.ProcessAttack();
                    _control.turbo = false;
                    yield return new WaitForSeconds(0.2f); //�� ����� ���������, ��� ����� ����� ���������
                    continue;
                }

                //Send PA
                if (_control.AICONTROLLER_DATA.aIConditions.CharacterIsGrounded(_control)
                    && _control.AICONTROLLER_DATA.aIConditions.CharacterIsGrounded(CharacterManager.Instance.GetPlayableCharacter())
                    && !_control.PLAYER_ANIMATION_DATA.IsRunning(typeof(JumpPrep)))
                {
                    _control.AICONTROLLER_DATA.aiStatus = Ai_Status.Sending_Pathfinding_Agent.ToString();
                    yield return StartCoroutine(_pathFindingAgent.ReinitAndSendPA(_control));
                }

                //Same platform
                if (_control.AICONTROLLER_DATA.aIConditions.TargetIsOnTheSamePlatform())
                {
                    Debug.Log("AIPROCESSOR: moving to start sphere");
                    yield return StartCoroutine(MoveToStartSphere_Routine(finishedMoveRoutine));
                }

                //Another Platform
                if (!_control.AICONTROLLER_DATA.aIConditions.TargetIsOnTheSamePlatform()
                    && _control.AICONTROLLER_DATA.aIConditions.CharacterIsGrounded(_control)
                    && !_control.PLAYER_ANIMATION_DATA.IsRunning(typeof(JumpPrep)))
                {
                    while (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToStartSphere() >= 1f)
                    {
                        yield return StartCoroutine(MoveToStartSphere_Routine(finishedMoveRoutine));
                        if (finishedMoveRoutine.Result == true)
                        {
                            _control.AICONTROLLER_DATA.aIBehavior.StopCharacter();
                            //yield return null;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if(finishedMoveRoutine.Result == false)
                    {
                        _control.AICONTROLLER_DATA.aIBehavior.StopCharacter();
                        yield return null;
                        continue;
                    }


                    Debug.Log("AIPROCESSOR: jump?");
                    if (_control.AICONTROLLER_DATA.aIConditions.EndSphereIsHigherThanStartSphere())
                    {
                        if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToStartSphere() <= _minimumDistanceToStartSphereForJump) //how close are we to the checkpoint    //����� ����� ������ ��� (����� ��� ������ ������ Start Point) ��-�� ������ �������� ��������� ��� �� ������������ ���� �����. ������ �� < 0.1f ��� ������
                        {
                            _control.AICONTROLLER_DATA.aIBehavior.StopCharacter();
                            _control.AICONTROLLER_DATA.aiStatus = Ai_Status.Jumping.ToString();
                            _control.jump = true;
                            _control.moveUp = true;
                            yield return StartCoroutine(OnJumpingToPlatform_Routine());
                        }
                        else if (_control.AICONTROLLER_DATA.aIConditions.CharacterIsGrounded(_control))
                        {
                            yield return StartCoroutine(MoveToStartSphere_Routine(finishedMoveRoutine));
                            if (finishedMoveRoutine.Result == true)
                            {
                                _control.AICONTROLLER_DATA.aIBehavior.StopCharacter();
                                //yield return null;
                            }
                            else
                            {
                                _control.AICONTROLLER_DATA.aIBehavior.StopCharacter();
                                yield return null;
                                continue;
                            }
                        }
                    }
                    else
                    {
                        _control.AICONTROLLER_DATA.aIBehavior.MoveToTheEndSphere();
                    }
                }

                //Stop if we reach target
                if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToEndSphere() < 1f)
                {
                    Debug.Log("AIPROCESSOT: REACHED END SPHERE!");
                    _control.AICONTROLLER_DATA.aIBehavior.StopCharacter();
                }

                //We should update spheres for keeping AI move
                if (_control.AICONTROLLER_DATA.aIConditions.TargetIsOnTheSamePlatform())
                {
                    Debug.Log("AIProcessor: UPDATING SPHERES POSITION!");
                    _control.AICONTROLLER_DATA.aIBehavior.ResetPASpheresPosition();
                }
                yield return null;
            }
        }

        private bool FrontBlockedByCharacter()
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

        private bool IsStartSphereOnSameY()
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

        private bool IsMoveToStartSphereCondition()
        {
            if (IsStartSphereOnSameY() == false || FrontBlockedByCharacter() == true)
            {
                return false;
            }

            return true;
        }

        private IEnumerator MoveToStartSphere_Routine(ResultWrapper<bool> finishedRoutine)
        {
            while (IsMoveToStartSphereCondition() == true)
            {
                _control.AICONTROLLER_DATA.aIBehavior.MoveToTheStartSphere();

                if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToStartSphere() <= _minimumDistanceToStartSphereForJump)
                {
                    finishedRoutine.Result = true;
                    _control.AICONTROLLER_DATA.aIBehavior.StopCharacter();
                    yield break;
                }
                yield return null;
            }
            finishedRoutine.Result = false;
            yield break;
        }
        private IEnumerator OnJumpingToPlatform_Routine()
        {

            _finishedToClimb = false;
            while (_finishedToClimb == false)
            {
                if (_control.PLAYER_ANIMATION_DATA.IsRunning(typeof(Landing)))
                {
                     yield break;
                }
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
                        _finishedToClimb = true;
                    }
                }
                yield return null;
            }
        }
    }
    class ResultWrapper<T>
    {
        public T Result { get; set; }
    }
}

