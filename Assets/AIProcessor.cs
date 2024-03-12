using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.GridLayoutGroup;

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
            Debug.Log("Started AI: " + _control.name);
            yield return null;

            _pathFindingAgent = _control.AICONTROLLER_DATA.pathfindingAgent;
            _pathFindingAgent.target = CharacterManager.Instance.GetPlayableCharacter().gameObject;

            yield return StartCoroutine(_pathFindingAgent.ReinitAndSendPA(_control));

            while (true)
            {
                Debug.Log("AIProcessot: new cycle");
                if (_pathFindingAgent == null)
                {
                    _pathFindingAgent = _control.AICONTROLLER_DATA.pathfindingAgent;
                }

                if (_control.PLAYER_ANIMATION_DATA.IsRunning(typeof(Landing)))
                {
                    _control.AICONTROLLER_DATA.aIBehavior.StopCharacter(); //get rid from MoveUp after jump
                    yield return null;
                    continue;
                }

                if (!_control.AICONTROLLER_DATA.aIConditions.CharacterIsGrounded(_control)
                    && !_control.AICONTROLLER_DATA.aIConditions.CharacterIsGrounded(CharacterManager.Instance.GetPlayableCharacter()))
                {
                    yield return null;
                    continue;
                }
                if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToTarget() <= 1f)
                {
                    _control.AICONTROLLER_DATA.aIBehavior.ProcessAttack();
                    _control.turbo = false;
                    yield return new WaitForSeconds(0.2f); //не делай маленьким, или пешка будет дергаться
                    continue;
                }



                if (_control.AICONTROLLER_DATA.aIConditions.CharacterIsGrounded(_control)
                    && _control.AICONTROLLER_DATA.aIConditions.CharacterIsGrounded(CharacterManager.Instance.GetPlayableCharacter())
                    && _aiHasReachedDestination
                    && _pathFindingAgent.hasFinishedPathfind
                    && !_control.PLAYER_ANIMATION_DATA.IsRunning(typeof(JumpPrep)))
                {
                    yield return StartCoroutine(_pathFindingAgent.ReinitAndSendPA(_control));
                }

                //Move
                Debug.Log("AIPROCESSOR: STARTING MOVING LOGIC");
                if (_control.AICONTROLLER_DATA.aIConditions.TargetIsOnTheSamePlatform())
                {
                    _control.AICONTROLLER_DATA.aIBehavior.MoveToTheStartSphere();

                    if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToStartSphere() <= 1f)
                    {
                        _aiHasReachedDestination = true;
                        _control.AICONTROLLER_DATA.aIBehavior.StopCharacter();
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
                //        if (_control.AICONTROLLER_DATA.aiLogistic.AIDistanceToStartSphere() < 0.08f) //how close are we to the checkpoint    //Здесь часто бывает баг (когда иди бегает вокруг Start Point) из-за разных смещений платформы или ИИ относительно друг друга. Увелич да < 0.1f для дебага
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

                yield return null;
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
                        _aiHasReachedDestination = true;
                        _finishedToClimb = true;
                    }
                }
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}

