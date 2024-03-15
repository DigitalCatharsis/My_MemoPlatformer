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
            _control.AICONTROLLER_DATA.aiStatus = Ai_Status.StartingAiProcessor.ToString();
            Debug.Log("Started AI: " + _control.name);
            yield return null;

            _pathFindingAgent = _control.AICONTROLLER_DATA.pathfindingAgent;
            _pathFindingAgent.target = CharacterManager.Instance.GetPlayableCharacter().gameObject;

            yield return StartCoroutine(_pathFindingAgent.ReinitAndSendPA(_control));

            while (true)
            {
                _control.AICONTROLLER_DATA.aiStatus = Ai_Status.RestartingProcessorCycle.ToString();
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
                    yield return new WaitForEndOfFrame();
                }

                if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToTarget() <= 1f)
                {
                    _control.AICONTROLLER_DATA.aIBehavior.ProcessAttack();
                    _control.turbo = false;
                    yield return new WaitForSeconds(0.2f); //не делай маленьким, или пешка будет дергаться
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
                    MoveToStartSphere();
                }

                //Another Platform
                if (!_control.AICONTROLLER_DATA.aIConditions.TargetIsOnTheSamePlatform())
                {
                    if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToStartSphere() <= 1f)
                    {
                        Debug.Log("AIPROCESSOR: jump?");
                        if (_control.AICONTROLLER_DATA.aIConditions.EndSphereIsHigherThanStartSphere())
                        {
                            if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToStartSphere() <= _minimumDistanceToStartSphereForJump) //how close are we to the checkpoint    //Здесь часто бывает баг (когда иди бегает вокруг Start Point) из-за разных смещений платформы или ИИ относительно друг друга. Увелич да < 0.1f для дебага
                            {
                                _control.AICONTROLLER_DATA.aIBehavior.StopCharacter();
                                _control.AICONTROLLER_DATA.aiStatus = Ai_Status.Jumping.ToString();
                                _control.jump = true;
                                _control.moveUp = true;
                                yield return StartCoroutine(OnJumpingToPlatform_Routine());
                            }
                            else if (_control.AICONTROLLER_DATA.aIConditions.CharacterIsGrounded(_control))
                            {
                                MoveToStartSphere();
                            }
                        }
                        else
                        {
                            _control.AICONTROLLER_DATA.aIBehavior.MoveToTheEndSphere();
                        }
                    }
                    else
                    {
                        Debug.Log("AIPROCESSOR: moving to start sphere");
                        MoveToStartSphere();
                    }
                }

                //Stop if we reach target
                if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToEndSphere() < 1f)
                {
                    Debug.Log("AIPROCESSOT: REACHED END SPHERE!");
                    _control.AICONTROLLER_DATA.aIBehavior.StopCharacter();
                }

                //If someone blocking - restart PA
                SetFrontBlockedCharacter();
                if (_control.AICONTROLLER_DATA.blockingCharacter != null)
                {
                    if (_control.GROUND_DATA.ground != null)
                    {
                        if (!_control.PLAYER_ANIMATION_DATA.IsRunning(typeof(Jump)) &&
                            !_control.PLAYER_ANIMATION_DATA.IsRunning(typeof(JumpPrep)))
                        {
                            _control.AICONTROLLER_DATA.aIBehavior.StopCharacter();
                            yield return null;
                            continue;
                        }
                    }
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

        private bool CheckForStartSphereHeightForWalkingCondition()
        {
            if (_control.GROUND_DATA.ground != null &&
                !_control.PLAYER_ANIMATION_DATA.IsRunning(typeof(Jump)) &&
                !_control.PLAYER_ANIMATION_DATA.IsRunning(typeof(WallJump_Prep)))
            {
                var height = _control.AICONTROLLER_DATA.aiLogistic.GetStartSphereHeight();
                if (height > 0.1f)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        private void MoveToStartSphere()
        {
            if (CheckForStartSphereHeightForWalkingCondition() == false)
            {
                _control.AICONTROLLER_DATA.aIBehavior.StopCharacter();
                return;
            }

            _control.AICONTROLLER_DATA.aIBehavior.MoveToTheStartSphere();

            if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToStartSphere() <= _minimumDistanceToStartSphereForJump)
            {
                _control.AICONTROLLER_DATA.aIBehavior.StopCharacter();
            }
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
                    //TODO Добавить проверку относительно стартовой сферы??

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
}

