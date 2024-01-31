using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

namespace My_MemoPlatformer
{
    public class BlockingObjects : SubComponent
    {
        public BlockingObj_Data blockingObj_Data;

        //Map each collided object to the collision detector
        private Dictionary<GameObject, GameObject> _frontBlockingObjects_dictionary = new Dictionary<GameObject, GameObject>(); //key refers to the sphere where the raycast is coming from, and value is the actual gameobject being hit 
        private Dictionary<GameObject, GameObject> _upBlockingObjects = new Dictionary<GameObject, GameObject>(); //key refers to the sphere where the raycast is coming from, and value is the actual gameobject being hit 
        private Dictionary<GameObject, GameObject> _downBlockingObjects = new Dictionary<GameObject, GameObject>(); //key refers to the sphere where the raycast is coming from, and value is the actual gameobject being hit 

        private List<CharacterControl> _airStompTargets = new List<CharacterControl>();

        //The entire list
        private List<GameObject> _frontBlockingObjList = new List<GameObject>();
        private List<GameObject> _frontBlockingCharacters = new List<GameObject>();

        private GameObject[] _frontSpheresArray;

        [Header("Setup blocking distance")]
        [Space(10)]
        [SerializeField] private float _frontBlocking_Distance;
        [SerializeField] private float _airStompDownBlocking_Distance;
        [SerializeField] private float _upBlocking_Distance;

        private void Start()
        {
            blockingObj_Data = new BlockingObj_Data
            {
                frontBlockingDictionaryCount = 0,
                upBlockingDictionaryCount = 0,

                frontBlocking_Distance = _frontBlocking_Distance,
                airStompDownBlocking_Distance = _airStompDownBlocking_Distance,
                upBlocking_Distance = _upBlocking_Distance,

                ClearFrontBlockingObjDic = ClearFrontBlockingObjDictionary,
                LeftSideBlocked = LeftSideIsBlocked,
                RightSideBLocked = RightSideIsBlocked,
                GetFrontBlockingCharactersList = GetFrontBlockingCharacterList,
                GetFrontBlockingObjList = GetFrontBlockingObjList,
            };

            subComponentProcessor.blockingObj_Data = blockingObj_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.BLOCKING_OBJECTS] = this;
        }

        public override void OnFixedUpdate()
        {
            if (Control.ANIMATION_DATA.IsRunning(typeof(MoveForward)))
            {
                if (!Control.DAMAGE_DATA.IsDead())
                {
                    DefineFrontSpheres();  //Consist of the side we a moving to
                    CheckFrontBlocking();
                }
            }
            else
            {
                if (_frontBlockingObjects_dictionary.Count != 0)
                {
                    _frontBlockingObjects_dictionary.Clear();
                }
            }

            //checking while ledge grabbing
            if (Control.ANIMATION_DATA.IsRunning(typeof(MoveUp)))
            {
                if (Control.animationProgress.latestMoveUpScript.speed > 0f)
                {
                    CheckUpBlockingAndAddToDictionary();
                }
            }
            else
            {
                //checking while player is jumping
                if (Control.RIGID_BODY.velocity.y > 0.001f)
                {
                    CheckUpBlockingAndAddToDictionary();

                    foreach (KeyValuePair<GameObject, GameObject> upBlockingObj_Data in _upBlockingObjects)
                    {
                        var characterControl = CharacterManager.Instance.GetCharacter(upBlockingObj_Data.Value.transform.gameObject);

                        if (characterControl == null)
                        {
                            Control.animationProgress.NullifyUpVelocity();
                            break;
                        }
                        else
                        {
                            if (Control.transform.position.y + Control.boxCollider.center.y < characterControl.transform.position.y)
                            {
                                Control.animationProgress.NullifyUpVelocity();
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (_upBlockingObjects.Count != 0)
                    {
                        _upBlockingObjects.Clear();
                    }
                }
            }

            CheckAndProcessAirStomp();

            blockingObj_Data.frontBlockingDictionaryCount = _frontBlockingObjects_dictionary.Count;
            blockingObj_Data.upBlockingDictionaryCount = _upBlockingObjects.Count;
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }

        private void CheckAndProcessAirStomp()
        {
            if (Control.RIGID_BODY.velocity.y >= 0f)
            {
                _airStompTargets.Clear();
                _downBlockingObjects.Clear();
                return;
            }

            if (_airStompTargets.Count > 0)
            {
                Control.RIGID_BODY.velocity = Vector3.zero;
                //TODO: Оптимизировать силу, вынести в поле
                Control.RIGID_BODY.AddForce(Vector3.up * 250f);

                foreach (var control in _airStompTargets)
                {
                    var attackCondition_Info = new AttackCondition();
                    attackCondition_Info.CopyInfo(control.DAMAGE_DATA.airStompAttack, base.Control);

                    var index = Random.Range(0, control.RAGDOLL_DATA.arrBodyPartsColliders.Length);
                    var randomPart = control.RAGDOLL_DATA.arrBodyPartsColliders[index].GetComponent<TriggerDetector>();

                    control.DAMAGE_DATA.damageTaken = new DamageTaken(
                        attacker: base.Control,
                        attack: control.DAMAGE_DATA.airStompAttack,
                        damaged_TG: randomPart,
                        damagerPart: base.Control.rightFoot_Attack,
                        incomingVelocity: Vector3.zero);

                    control.DAMAGE_DATA.TakeDamage(attackCondition_Info);
                }

                _airStompTargets.Clear();
                return;
            }

            CheckDownBlocking();

            if (_downBlockingObjects.Count > 0)
            {
                foreach (KeyValuePair<GameObject, GameObject> data in _downBlockingObjects)
                {
                    var control = CharacterManager.Instance.GetCharacter(data.Value.transform.gameObject);

                    if (control != null)
                    {
                        if (control.boxCollider.center.y + control.transform.position.y < Control.transform.position.y)
                        {
                            if (control != Control)
                            {
                                if (!_airStompTargets.Contains(control))
                                {
                                    _airStompTargets.Add(control);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CheckFrontBlocking()  //Проверка на коллизии
        {
            if (DebugContainer_Data.Instance.debug_Colliders)
            {
                Debug.Log("Checking Front Blocking (BlockingObject.CS");
            }

            for (int i = 0; i < _frontSpheresArray.Length; i++)
            {
                var blockingObj = CollisionDetection.GetCollidingObject(
                    Control, _frontSpheresArray[i], this.transform.forward * blockingObj_Data.frontBlocking_Distance,
                    Control.animationProgress.latestMoveForwardScript.blockDistance,
                    ref Control.BLOCKING_OBJ_DATA.raycastContactPoint);

                if (blockingObj != null)
                {
                    AddBlockingObjToDictionary(_frontBlockingObjects_dictionary, _frontSpheresArray[i], blockingObj);
                }
                else
                {
                    RemoveBlockingObjFromDictionary(_frontBlockingObjects_dictionary, _frontSpheresArray[i]);
                }
            }
        }
        private void DefineFrontSpheres()         //Consist of the side we a moving to
        {
            if (!Control.animationProgress.IsForwardReversed())
            {
                _frontSpheresArray = Control.COLLISION_SPHERE_DATA.frontSpheres;
                blockingObj_Data.frontBlocking_Distance = _frontBlocking_Distance;

                RemoveSpheresInArrayFromDictionary(Control.COLLISION_SPHERE_DATA.backSpheres, _frontBlockingObjects_dictionary);
            }
            else
            {
                _frontSpheresArray = Control.COLLISION_SPHERE_DATA.backSpheres;
                blockingObj_Data.frontBlocking_Distance = -_frontBlocking_Distance;

                RemoveSpheresInArrayFromDictionary(Control.COLLISION_SPHERE_DATA.frontSpheres, _frontBlockingObjects_dictionary);
            }
        }

        private void RemoveSpheresInArrayFromDictionary(GameObject[] spheresArray, Dictionary<GameObject, GameObject> spheresDictionary)
        {
            foreach (var s in spheresArray)
            {
                if (spheresDictionary.ContainsKey(s))
                {
                    spheresDictionary.Remove(s);
                }
            }
        }

        private void CheckDownBlocking()
        {
            foreach (var sphere in Control.COLLISION_SPHERE_DATA.bottomSpheres)
            {
                GameObject blockingObj = CollisionDetection.GetCollidingObject(Control, sphere, Vector3.down, blockingObj_Data.airStompDownBlocking_Distance, ref Control.BLOCKING_OBJ_DATA.raycastContactPoint);

                if (blockingObj != null)
                {
                    AddBlockingObjToDictionary(_downBlockingObjects, sphere, blockingObj);
                }
                else
                {
                    RemoveBlockingObjFromDictionary(_downBlockingObjects, sphere);
                }
            }
        }

        private void CheckUpBlockingAndAddToDictionary()
        {
            foreach (var o in Control.COLLISION_SPHERE_DATA.upSpheres)
            {
                var blockingObj = CollisionDetection.GetCollidingObject(Control, o, this.transform.up, blockingObj_Data.upBlocking_Distance, ref Control.BLOCKING_OBJ_DATA.raycastContactPoint);

                if (blockingObj != null)
                {
                    AddBlockingObjToDictionary(_upBlockingObjects, o, blockingObj);
                }
                else
                {
                    RemoveBlockingObjFromDictionary(_upBlockingObjects, o);
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

        private bool RightSideIsBlocked()
        {
            foreach (KeyValuePair<GameObject, GameObject> data in _frontBlockingObjects_dictionary)
            {
                if ((data.Value.transform.position - Control.transform.position).z > 0f)
                {
                    return true;
                }
            }
            return false;
        }

        private bool LeftSideIsBlocked()
        {
            foreach (KeyValuePair<GameObject, GameObject> data in _frontBlockingObjects_dictionary)
            {
                if ((data.Value.transform.position - Control.transform.position).z < 0f)
                {
                    return true;
                }
            }
            return false;
        }

        private void ClearFrontBlockingObjDictionary()
        {
            _frontBlockingObjects_dictionary.Clear();
        }

        private List<GameObject> GetFrontBlockingCharacterList()
        {
            _frontBlockingCharacters.Clear();

            foreach (KeyValuePair<GameObject, GameObject> data in _frontBlockingObjects_dictionary)
            {
                var control = CharacterManager.Instance.GetCharacter(data.Value.transform.gameObject);

                if (control != null)
                {
                    if (!_frontBlockingCharacters.Contains(control.gameObject))
                    {
                        _frontBlockingCharacters.Add(control.gameObject);
                    }
                }
            }

            return _frontBlockingCharacters;
        }

        private List<GameObject> GetFrontBlockingObjList()
        {
            _frontBlockingObjList.Clear();

            foreach (KeyValuePair<GameObject, GameObject> data in _frontBlockingObjects_dictionary)
            {
                if (!_frontBlockingObjList.Contains(data.Value))
                {
                    _frontBlockingObjList.Add(data.Value);
                }
            }

            return _frontBlockingObjList;
        }
    }
}