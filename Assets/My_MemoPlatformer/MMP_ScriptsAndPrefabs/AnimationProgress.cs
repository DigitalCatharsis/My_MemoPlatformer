using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace My_MemoPlatformer
{
    public class AnimationProgress : MonoBehaviour
    {
        public Dictionary<StateData, int> currentRunningAbilities = new Dictionary<StateData, int>();

        public bool cameraShaken;
        public List<PoolObjectType> poolObjectList = new List<PoolObjectType>();
        public MoveForward latestMoveForwardScript;  //latest moveforward script
        public MoveUp latestMoveUpScript;  //latest moveforward script
        private List<GameObject> _frontSpheresList;

        [Header("Attack Button")]
        public bool attackTriggered;
        public bool attackButtonIsReset;


        [Header("GroundMovement")]
        public bool disAllowEarlyTurn;
        public bool lockDirectionNextState;
        public bool isIgnoreCharacterTime; //slide beyond character (start ignoring character collider)
        private float _dirBlock;

        [Header("Colliding Objects")]
        public GameObject ground;
        public Dictionary<TriggerDetector, List<Collider>> collidingBodyParts = new Dictionary<TriggerDetector, List<Collider>>(); //key trigger detectors, value - colliding bodyparts which are in contract with trigger detectors
        public Dictionary<TriggerDetector, List<Collider>> collidingWeapons = new Dictionary<TriggerDetector, List<Collider>>(); //key trigger detectors, value - colliding bodyparts which are in contract with trigger detectors

        public Dictionary<GameObject, GameObject> frontBlockingObjects = new Dictionary<GameObject, GameObject>(); //key refers to the sphere where the raycast is coming from, and value is the actual gameobject being hit 
        public Dictionary<GameObject, GameObject> upBlockingObjects = new Dictionary<GameObject, GameObject>(); //key refers to the sphere where the raycast is coming from, and value is the actual gameobject being hit 
        public Dictionary<GameObject, GameObject> downBlockingObjects = new Dictionary<GameObject, GameObject>(); //key refers to the sphere where the raycast is coming from, and value is the actual gameobject being hit 

        public Vector3 collidingPoint = new Vector3();

        [Header("AirControl")]
        public bool jumped;
        public float airMomentum;
        public Vector3 maxFallVelocity;
        public bool cancelPull;
        public bool canWallJump;
        public bool checkWallBlock;
        public List<CharacterControl> airStompTargets;

        [Header("UpdateBoxCollider")]
        public bool updatingSpheres;
        public Vector3 targetSize;
        public float sizeSpeed;
        public Vector3 targetCenter;
        public float centerSpeed;
        public Vector3 landingPosition;
        public bool isLanding;

        [Header("Transition")]
        public bool lockTransition;

        [Header("Weapon")]
        public MeleeWeapon HoldingWeapon;

        private CharacterControl _control;

        private void Awake()
        {
            _control = GetComponentInParent<CharacterControl>();
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

        private void FixedUpdate()
        {
            if (IsRunning(typeof(MoveForward)))
            {
                CheckFrontBlocking();
            }
            else
            {
                if (frontBlockingObjects.Count != 0)
                {
                    frontBlockingObjects.Clear();
                }
            }

            //checking while ledge grabbing
            if (IsRunning(typeof(MoveUp)))
            {
                if (latestMoveUpScript.speed > 0f)
                {
                    CheckUpBlocking();
                }
            }
            else
            {
                //checking while player is jumping
                if (_control.Rigid_Body.velocity.y > 0.001f)
                {
                    CheckUpBlocking();

                    foreach (KeyValuePair<GameObject, GameObject> data in upBlockingObjects)
                    {
                        var c = CharacterManager.Instance.GetCharacter(data.Value.transform.root.gameObject);

                        if (c == null)
                        {
                            NullifyUpVelocity();
                            break;
                        }
                        else
                        {
                            if (_control.transform.position.y + _control.boxCollider.center.y < c.transform.position.y)
                            {
                                NullifyUpVelocity();
                                break;
                            }
                        }
                    }

                    if (upBlockingObjects.Count > 0)
                    {
                        _control.Rigid_Body.velocity = new Vector3(_control.Rigid_Body.velocity.x, 0f, _control.Rigid_Body.velocity.z);
                    }
                }
                else
                {
                    if (upBlockingObjects.Count != 0)
                    {
                        upBlockingObjects.Clear();
                    }
                }
            }

            CheckAirStomp();
        }

        private void NullifyUpVelocity()
        {
            _control.Rigid_Body.velocity = new Vector3(
                _control.Rigid_Body.velocity.x,
                0f,
                _control.Rigid_Body.velocity.z);
        }

        private void CheckAirStomp()
        {
            if (_control.Rigid_Body.velocity.y >= 0f)
            {
                airStompTargets.Clear();
                downBlockingObjects.Clear();
                return;
            }

            if (airStompTargets.Count > 0)
            {
                _control.Rigid_Body.velocity = Vector3.zero;
                _control.Rigid_Body.AddForce(Vector3.up * 350f);

                foreach (CharacterControl c in airStompTargets)
                {
                    var info = new AttackInfo();
                    info.CopyInfo(c.damageDetector.airStompAttack, _control);

                    int index = Random.Range(0, c.bodyParts.Count);
                    c.damageDetector.damagedTrigger = c.bodyParts[index].GetComponent<TriggerDetector>();
                    c.damageDetector.attack = c.damageDetector.airStompAttack;
                    c.damageDetector.attacker = _control;
                    c.damageDetector.attackingPart = _control.rightFoot_Attack;

                    c.damageDetector.TakeDamage(info);
                }

                airStompTargets.Clear();
                return;
            }

            CheckDownBlocking();

            if (downBlockingObjects.Count > 0)
            {
                foreach (KeyValuePair<GameObject, GameObject> data in downBlockingObjects)
                {
                    CharacterControl c = CharacterManager.Instance.GetCharacter(data.Value.transform.root.gameObject);
                    if (c != null)
                    {
                        if (c.boxCollider.center.y + c.transform.position.y < _control.transform.position.y)
                        {
                            if (c != _control)  //not self
                            {
                                if (!airStompTargets.Contains(c))
                                {
                                    airStompTargets.Add(c);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AddBlockingObjToDictionary(Dictionary<GameObject, GameObject> dic, GameObject key, GameObject value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }
        }

        private void RemoveBlockingObjFromDictionary(Dictionary<GameObject, GameObject> dic, GameObject key)
        {
            if (dic.ContainsKey(key))
            {
                dic.Remove(key);
            }
        }

        public bool IsFacingAtacker()
        {
            var vec = _control.damageDetector.attacker.transform.position - _control.transform.position;

            if (vec.z < 0f)
            {
                if (_control.IsFacingForward())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (vec.z > 0f)
            {
                if (_control.IsFacingForward())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private bool ForwardIsReversed()
        {
            if (latestMoveForwardScript.moveOnHit)
            {
                if (IsFacingAtacker())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (latestMoveForwardScript.speed > 0f)
            {
                return false;
            }
            else if (latestMoveForwardScript.speed < 0f)
            {
                return true;
            }

            return false;
        }
        private void CheckUpBlocking()
        {
            foreach (var o in _control.collisionSpheres.upSpheres)
            {
                var blockingObj = CollisionDetection.GetCollidingObject(_control, o, this.transform.up, 0.3f, ref _control.animationProgress.collidingPoint);

                if (blockingObj != null)
                {
                    AddBlockingObjToDictionary(upBlockingObjects, o, blockingObj);
                }
                else
                {
                    RemoveBlockingObjFromDictionary(upBlockingObjects, o);
                }
            }
        }

        private void CheckDownBlocking()
        {
            foreach (var o in _control.collisionSpheres.bottomSpheres)
            {
                var blockingObj = CollisionDetection.GetCollidingObject(_control, o, Vector3.down, 0.1f, ref _control.animationProgress.collidingPoint);

                if (blockingObj != null)
                {
                    AddBlockingObjToDictionary(downBlockingObjects, o, blockingObj);
                }
                else
                {
                    RemoveBlockingObjFromDictionary(downBlockingObjects, o);
                }
            }
        }

        private void CheckFrontBlocking()  //Проверка на коллизии
        {
            if (!ForwardIsReversed())
            {
                _frontSpheresList = _control.collisionSpheres.frontSpheres;
                _dirBlock = 1f;

                foreach (var sphere in _control.collisionSpheres.backSpheres)
                {
                    if (frontBlockingObjects.ContainsKey(sphere))
                    {
                        frontBlockingObjects.Remove(sphere);
                    }
                }
            }
            else
            {
                _frontSpheresList = _control.collisionSpheres.backSpheres;
                _dirBlock = -1;

                foreach (GameObject sphere in _control.collisionSpheres.frontSpheres)
                {
                    if (frontBlockingObjects.ContainsKey(sphere))
                    {
                        frontBlockingObjects.Remove(sphere);
                    }
                }
            }

            foreach (var o in _frontSpheresList)
            {
                var blockingObj = CollisionDetection.GetCollidingObject(_control, o, this.transform.forward * _dirBlock, latestMoveForwardScript.blockDistance, ref _control.animationProgress.collidingPoint);

                if (blockingObj != null)
                {
                    AddBlockingObjToDictionary(frontBlockingObjects, o, blockingObj);
                }
                else
                {
                    RemoveBlockingObjFromDictionary(frontBlockingObjects, o);
                }
            }
        }

        public bool StateNameContains(string str)
        {
            AnimatorClipInfo[] arr = _control.skinnedMeshAnimator.GetCurrentAnimatorClipInfo(0); //have only one layer which is zero

            foreach (var clipinfo in arr)
            {
                if (clipinfo.clip.name.Contains(str))
                {
                    return true;
                }
            }

            return false;
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
            foreach (KeyValuePair<GameObject, GameObject> data in frontBlockingObjects)
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
            foreach (KeyValuePair<GameObject, GameObject> data in frontBlockingObjects)
            {
                if ((data.Value.transform.position - _control.transform.position).z < 0f)
                {
                    return true;
                }
            }
            return false;
        }

        public MeleeWeapon GetTouchingWeapon()
        {
            foreach (KeyValuePair<TriggerDetector, List<Collider>> data in collidingWeapons)
            {
                var w = data.Value[0].gameObject.GetComponent<MeleeWeapon>();
                return w;
            }

            return null;
        }
    }
}
