using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AnimationProgress : MonoBehaviour
    {
        public Dictionary<StateData, int> currentRunningAbilities = new Dictionary<StateData, int>();

        public bool cameraShaken;
        public List<PoolObjectType> poolObjectList = new List<PoolObjectType>();
        public bool ragdollTriggered;
        public MoveForward latestMoveForwardScript;  //latest moveforward script

        [Header("Attack Button")]
        public bool attackTriggered;
        public bool attackButtonIsReset;


        [Header("GroundMovement")]
        public bool disAllowEarlyTurn;
        public bool lockDirectionNextState;
        public bool isIgnoreCharacterTime; //slide beyond character (start ignoring character collider)
        private List<GameObject> _spheresList;
        private float _dirBlock;

        [Header("Colliding Objects")]
        public GameObject ground;
        public Dictionary<TriggerDetector, List<Collider>> collidingBodyParts = new Dictionary<TriggerDetector, List<Collider>>(); //key trigger detectors, value - colliding bodyparts which are in contract with trigger detectors
        public Dictionary<GameObject, GameObject> blockingObjects = new Dictionary<GameObject, GameObject>(); //key refers to the sphere where the raycast is coming from, and value is the actual gameobject being hit 

        [Header("AirControl")]
        public bool jumped;
        public float airMomentum;
        public Vector3 maxFallVelocity;
        public bool cancelPull;
        public bool canWallJump;
        public bool checkWallBlock;

        [Header("UpdateBoxCollider")]
        public bool updatingSpheres;
        public Vector3 targetSize;
        public float sizeSpeed;
        public Vector3 targetCenter;
        public float centerSpeed;

        [Header("Damage Info")]
        public Attack attack;
        public CharacterControl attacker;
        public TriggerDetector damagedTrigger;
        public GameObject attackingPart;

        [Header("Transition")]
        public bool lockTransition;

        private CharacterControl _control;

        private void Awake()
        {
            _control = GetComponentInParent<CharacterControl>();
        }

        private void FixedUpdate()
        {
            if (IsRunning(typeof(MoveForward)))
            {
                CheckForBlockingObjects();
            }
            else
            {
                if (blockingObjects.Count != 0)
                {
                    blockingObjects.Clear();
                }
            }
        }

        private void CheckForBlockingObjects()  //Проверка на коллизии
        {
            if (latestMoveForwardScript.speed > 0)
            {
                _spheresList = _control.collisionSpheres.frontSpheres;
                _dirBlock = 0.3f;

                foreach (GameObject sphere in _control.collisionSpheres.backSpheres)
                {
                    if (blockingObjects.ContainsKey(sphere))
                    {
                        blockingObjects.Remove(sphere);
                    }
                }
            }
            else
            {

                _spheresList = _control.collisionSpheres.backSpheres;
                _dirBlock = -0.3f;

                foreach (GameObject sphere in _control.collisionSpheres.frontSpheres)
                {
                    if (blockingObjects.ContainsKey(sphere))
                    {
                        blockingObjects.Remove(sphere);
                    }
                }
            }

            foreach (GameObject o in _spheresList)
            {
                Debug.DrawRay(o.transform.position, _control.transform.forward * _dirBlock, Color.yellow);
                RaycastHit hit;
                if (Physics.Raycast(o.transform.position, _control.transform.forward * _dirBlock, out hit, latestMoveForwardScript.blockDistance))
                {
                    if (!IsBodyPart(hit.collider)
                        && !IsIgnoringCharacter(hit.collider)
                        && !Ledge.IsLedge(hit.collider.gameObject)
                          && !Ledge.IsLedgeChecker(hit.collider.gameObject))  // Проверка, что мы ничего не задеваем, включая Ledge (платформы, за котоыре можно зацепиться)
                    {
                        if (blockingObjects.ContainsKey(o)) //Если сфера есть в списке
                        {
                            blockingObjects[o] = hit.collider.transform.root.gameObject; //Добавляем объект, который ее колайдит
                        }
                        else
                        {
                            blockingObjects.Add(o, hit.collider.transform.root.gameObject);
                        }
                    }
                    else //not match conditions
                    {
                        if (blockingObjects.ContainsKey(o))
                        {
                            blockingObjects.Remove(o);
                        }
                    }
                }
                else  //collide nothing
                {
                    if (blockingObjects.ContainsKey(o))
                    {
                        blockingObjects.Remove(o);
                    }
                }
            }
        }

        private bool IsIgnoringCharacter(Collider col)
        {
            if (!isIgnoreCharacterTime)
            {
                return false;
            }
            else
            {
                CharacterControl blockingCharacter = CharacterManager.Instance.GetCharacter(col.transform.root.root.gameObject);

                if (blockingCharacter == null)
                {
                    return false;
                }

                if (blockingCharacter == _control)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        private bool IsBodyPart(Collider col)
        {
            if (col.transform.root.gameObject == _control.gameObject)
            {
                return true;
            }


            CharacterControl target = CharacterManager.Instance.GetCharacter(col.transform.root.gameObject);

            if (target == null)
            {
                return false;
            }

            if (target.damageDetector.damageTaken > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void Update()
        {
            if (_control.attack)
            { //dont trigger attack several times
                if (attackButtonIsReset)
                {
                    attackTriggered = true;
                    attackButtonIsReset = false;
                }
            }
            else
            {
                attackButtonIsReset = true;
                attackTriggered = false;
            }

            if (IsRunning(typeof(LockTransition)))
            {
                if (_control.animationProgress.lockTransition)
                {
                    _control.skinnedMeshAnimator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.LockTransition], true);
                }
                else
                {
                    _control.skinnedMeshAnimator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.LockTransition], false);
                }
            }
            else
            {
                _control.skinnedMeshAnimator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.LockTransition], false);
            }
        }

        public bool IsRunning(System.Type type) //ability is running now?
        {
            foreach (KeyValuePair<StateData, int> data in currentRunningAbilities)
            {
                if (data.Key.GetType() == type)
                {
                    return true;
                }
            }
            return false;
        }

        public bool RightSideIsBlocked()
        {
            foreach (KeyValuePair<GameObject, GameObject> data in blockingObjects)
            {
                if ((data.Value.transform.position - _control.transform.position).z > 0f)
                {
                    return true;
                }
            }
            return false;
        }

        public bool LeftSideIsBlocked()
        {
            foreach (KeyValuePair<GameObject, GameObject> data in blockingObjects)
            {
                if ((data.Value.transform.position - _control.transform.position).z < 0f)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
