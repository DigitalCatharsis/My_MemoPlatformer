using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class BlockingObjects : SubComponent
    {
        public BlockingObj_Data blockingObj_Data;

        //Map each collided object to the collision detector
        private Dictionary<GameObject, GameObject> _frontBlockingObjects = new Dictionary<GameObject, GameObject>(); //key refers to the sphere where the raycast is coming from, and value is the actual gameobject being hit 
        private Dictionary<GameObject, GameObject> _upBlockingObjects = new Dictionary<GameObject, GameObject>(); //key refers to the sphere where the raycast is coming from, and value is the actual gameobject being hit 
        private Dictionary<GameObject, GameObject> _downBlockingObjects = new Dictionary<GameObject, GameObject>(); //key refers to the sphere where the raycast is coming from, and value is the actual gameobject being hit 

        private List<CharacterControl> _airStompTargets = new List<CharacterControl>();

        //The entire list
        private List<GameObject> _frontBlockingObjList = new List<GameObject>();
        private List<GameObject> _frontBlockingCharacters = new List<GameObject>();

        private GameObject[] _frontSpheresArray;

        private float _dirBlock;

        private void Start()
        {
            blockingObj_Data = new BlockingObj_Data
            {
                frontBlockingDictionaryCount = 0,
                upBlockingDictionaryCount = 0,
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
            if (control.ANIMATION_DATA.IsRunning(typeof(MoveForward)))
            {
                CheckFrontBlocking();
            }
            else
            {
                if (_frontBlockingObjects.Count != 0)
                {
                    _frontBlockingObjects.Clear();
                }
            }

            //checking while ledge grabbing
            if (control.ANIMATION_DATA.IsRunning(typeof(MoveUp)))
            {
                if (control.animationProgress.latestMoveUpScript.speed > 0f)
                {
                    CheckUpBlocking();
                }
            }
            else
            {
                //checking while player is jumping
                if (control.RIGID_BODY.velocity.y > 0.001f)
                {
                    CheckUpBlocking();

                    foreach (KeyValuePair<GameObject, GameObject> data in _upBlockingObjects)
                    {
                        var c = CharacterManager.Instance.GetCharacter(data.Value.transform.root.gameObject);

                        if (c == null)
                        {
                            control.animationProgress.NullifyUpVelocity();
                            break;
                        }
                        else
                        {
                            if (control.transform.position.y + control.boxCollider.center.y < c.transform.position.y)
                            {
                                control.animationProgress.NullifyUpVelocity();
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

            CheckAirStomp();

            blockingObj_Data.frontBlockingDictionaryCount = _frontBlockingObjects.Count;
            blockingObj_Data.upBlockingDictionaryCount = _upBlockingObjects.Count;
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }

        private void CheckAirStomp()
        {
            if (control.RIGID_BODY.velocity.y >= 0f)
            {
                _airStompTargets.Clear();
                _downBlockingObjects.Clear();
                return;
            }

            if (_airStompTargets.Count > 0)
            {
                control.RIGID_BODY.velocity = Vector3.zero;
                control.RIGID_BODY.AddForce(Vector3.up * 250f);

                foreach (var c in _airStompTargets)
                {
                    var info = new AttackCondition();
                    info.CopyInfo(c.DAMAGE_DATA.airStompAttack, control);

                    int index = Random.Range(0, c.RAGDOLL_DATA.arrBodyParts.Length);
                    TriggerDetector randomPart = c.RAGDOLL_DATA.arrBodyParts[index].GetComponent<TriggerDetector>();

                    c.DAMAGE_DATA.damageTaken = new DamageTaken(
                        control,
                        c.DAMAGE_DATA.airStompAttack,
                        randomPart,
                        control.rightFoot_Attack,
                        Vector3.zero);

                    c.DAMAGE_DATA.TakeDamage(info);
                }

                _airStompTargets.Clear();
                return;
            }

            CheckDownBlocking();

            if (_downBlockingObjects.Count > 0)
            {
                foreach (KeyValuePair<GameObject, GameObject> data in _downBlockingObjects)
                {
                    var c = CharacterManager.Instance.GetCharacter(data.Value.transform.root.gameObject);

                    if (c != null)
                    {
                        if (c.boxCollider.center.y + c.transform.position.y < control.transform.position.y)
                        {
                            if (c != control)
                            {
                                if (!_airStompTargets.Contains(c))
                                {
                                    _airStompTargets.Add(c);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CheckFrontBlocking()  //Проверка на коллизии
        {
            if (!control.animationProgress.ForwardIsReversed())
            {
                _frontSpheresArray = control.COLLISION_SPHERE_DATA.frontSpheres;
                _dirBlock = 1f;

                foreach (var s in control.COLLISION_SPHERE_DATA.backSpheres)
                {
                    if (_frontBlockingObjects.ContainsKey(s))
                    {
                        _frontBlockingObjects.Remove(s);
                    }
                }
            }
            else
            {
                _frontSpheresArray = control.COLLISION_SPHERE_DATA.backSpheres;
                _dirBlock = -1f;

                foreach (var s in control.COLLISION_SPHERE_DATA.frontSpheres)
                {
                    if (_frontBlockingObjects.ContainsKey(s))
                    {
                        _frontBlockingObjects.Remove(s);
                    }
                }
            }

            for (int i = 0; i < _frontSpheresArray.Length; i++)
            {
                GameObject blockingObj = CollisionDetection.GetCollidingObject(
                    control, _frontSpheresArray[i], this.transform.forward * _dirBlock,
                    control.animationProgress.latestMoveForwardScript.blockDistance,
                    ref control.BLOCKING_OBJ_DATA.raycastContactPoint);

                if (blockingObj != null)
                {
                    AddBlockingObjToDictionary(_frontBlockingObjects, _frontSpheresArray[i], blockingObj);
                }
                else
                {
                    RemoveBlockingObjFromDictionary(_frontBlockingObjects, _frontSpheresArray[i]);
                }
            }
        }

        private void CheckDownBlocking()
        {
            foreach (var sphere in control.COLLISION_SPHERE_DATA.bottomSpheres)
            {
                GameObject blockingObj = CollisionDetection.GetCollidingObject(control, sphere, Vector3.down, 0.1f, ref control.BLOCKING_OBJ_DATA.raycastContactPoint);

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

        private void CheckUpBlocking()
        {
            foreach (var o in control.COLLISION_SPHERE_DATA.upSpheres)
            {
                GameObject blockingObj = CollisionDetection.GetCollidingObject(control, o, this.transform.up, 0.3f, ref control.BLOCKING_OBJ_DATA.raycastContactPoint);

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
            foreach (KeyValuePair<GameObject, GameObject> data in _frontBlockingObjects)
            {
                if ((data.Value.transform.position - control.transform.position).z > 0f)
                {
                    return true;
                }
            }
            return false;
        }

        private bool LeftSideIsBlocked()
        {
            foreach (KeyValuePair<GameObject, GameObject> data in _frontBlockingObjects)
            {
                if ((data.Value.transform.position - control.transform.position).z < 0f)
                {
                    return true;
                }
            }
            return false;
        }

        private void ClearFrontBlockingObjDictionary()
        {
            _frontBlockingObjects.Clear();
        }

        private List<GameObject> GetFrontBlockingCharacterList()
        {
            _frontBlockingCharacters.Clear();

            foreach (KeyValuePair<GameObject, GameObject> data in _frontBlockingObjects)
            {
                CharacterControl c = CharacterManager.Instance.GetCharacter(data.Value.transform.root.gameObject);

                if (c != null)
                {
                    if (!_frontBlockingCharacters.Contains(c.gameObject))
                    {
                        _frontBlockingCharacters.Add(c.gameObject);
                    }
                }
            }

            return _frontBlockingCharacters;
        }

        private List<GameObject> GetFrontBlockingObjList()
        {
            _frontBlockingObjList.Clear();

            foreach (KeyValuePair<GameObject, GameObject> data in _frontBlockingObjects)
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