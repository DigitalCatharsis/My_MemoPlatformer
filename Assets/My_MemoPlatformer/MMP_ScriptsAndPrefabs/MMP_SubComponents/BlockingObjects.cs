using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class BlockingObjects : SubComponent
    {
        //Map each collided object to the collision detector
        public Dictionary<GameObject, GameObject> frontBlockingObjects = new Dictionary<GameObject, GameObject>(); //key refers to the sphere where the raycast is coming from, and value is the actual gameobject being hit 
        public Dictionary<GameObject, GameObject> upBlockingObjects = new Dictionary<GameObject, GameObject>(); //key refers to the sphere where the raycast is coming from, and value is the actual gameobject being hit 
        public Dictionary<GameObject, GameObject> downBlockingObjects = new Dictionary<GameObject, GameObject>(); //key refers to the sphere where the raycast is coming from, and value is the actual gameobject being hit 

        //The entire list
        private List<GameObject> _frontBlockingObjList = new List<GameObject>();

        public List<CharacterControl> airStompTargets = new List<CharacterControl>();        
        private List<GameObject> _frontBlockingCharacters = new List<GameObject>();
        private List<GameObject> _frontSpheresList;

        private float _dirBlock;

        private void Start()
        {
            subComponentProcessor.componentsDictionary.Add(SubComponents.BLOCKINGOBJECTS, this);
            control.procDict.Add(CharacterProc.CLEAR_FRONTBLOCKING_OBJ_DICTIONARY, ClearFrontBlockingObjDictionary);

            control.boolDic.Add(BoolData.UPBLOCKINGOBJ_DICTIONARY_IS_EMPTY, UpBlockingObjDictionaryIsEmpty);
            control.boolDic.Add(BoolData.FRONTBLOCKINGOBJ_DICTIONARY_IS_EMPTY, FrontBlockingObjDictionaryIsEmpty);
            control.boolDic.Add(BoolData.RIGHTSIDE_IS_BLOCKED, RightSideIsBlocked);
            control.boolDic.Add(BoolData.LEFTSIDE_IS_BLOCKED, LeftSideIsBlocked);

            control.listDic.Add(ListData.FRONTBLOCKING_CHARACTERS, GetFrontBlockingCharacters);
            control.listDic.Add(ListData.FRONTBLOCKING_OBJECTS, GetFrontBlockingObjList);
        }

        public override void OnFixedUpdate()
        {
            if (control.animationProgress.IsRunning(typeof(MoveForward)))
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
            if (control.animationProgress.IsRunning(typeof(MoveUp)))
            {
                if (control.animationProgress.latestMoveUpScript.speed > 0f)
                {
                    CheckUpBlocking();
                }
            }
            else
            {
                //checking while player is jumping
                if (control.Rigid_Body.velocity.y > 0.001f)
                {
                    CheckUpBlocking();

                    foreach (KeyValuePair<GameObject, GameObject> data in upBlockingObjects)
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

                    if (upBlockingObjects.Count > 0)
                    {
                        control.Rigid_Body.velocity = new Vector3(control.Rigid_Body.velocity.x, 0f, control.Rigid_Body.velocity.z);
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

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }

        private void CheckAirStomp()
        {
            if (control.Rigid_Body.velocity.y >= 0f)
            {
                airStompTargets.Clear();
                downBlockingObjects.Clear();
                return;
            }

            if (airStompTargets.Count > 0)
            {
                control.Rigid_Body.velocity = Vector3.zero;
                control.Rigid_Body.AddForce(Vector3.up * 350f);

                foreach (var c in airStompTargets)
                {
                    var info = new AttackInfo();
                    info.CopyInfo(c.damageDetector.airStompAttack, control);

                    int index = Random.Range(0, c.bodyParts.Count);
                    c.damageDetector.damagedTrigger = c.bodyParts[index].GetComponent<TriggerDetector>();
                    c.damageDetector.attack = c.damageDetector.airStompAttack;
                    c.damageDetector.attacker = control;
                    c.damageDetector.attackingPart = control.rightFoot_Attack;

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
                    var c = CharacterManager.Instance.GetCharacter(data.Value.transform.root.gameObject);
                    if (c != null)
                    {
                        if (c.boxCollider.center.y + c.transform.position.y < control.transform.position.y)
                        {
                            if (c != control)  //not self
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

        private void CheckFrontBlocking()  //Проверка на коллизии
        {
            if (!control.animationProgress.ForwardIsReversed())
            {
                _frontSpheresList = control.collisionSpheres.frontSpheres;
                _dirBlock = 1f;

                foreach (var sphere in control.collisionSpheres.backSpheres)
                {
                    if (frontBlockingObjects.ContainsKey(sphere))
                    {
                        frontBlockingObjects.Remove(sphere);
                    }
                }
            }
            else
            {
                _frontSpheresList = control.collisionSpheres.backSpheres;
                _dirBlock = -1;

                foreach (GameObject sphere in control.collisionSpheres.frontSpheres)
                {
                    if (frontBlockingObjects.ContainsKey(sphere))
                    {
                        frontBlockingObjects.Remove(sphere);
                    }
                }
            }

            foreach (var o in _frontSpheresList)
            {
                var blockingObj = CollisionDetection.GetCollidingObject(control, o, this.transform.forward * _dirBlock, control.animationProgress.latestMoveForwardScript.blockDistance, ref control.animationProgress.collidingPoint);

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


        private void CheckUpBlocking()
        {
            foreach (var o in control.collisionSpheres.upSpheres)
            {
                var blockingObj = CollisionDetection.GetCollidingObject(control, o, this.transform.up, 0.3f, ref control.animationProgress.collidingPoint);

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
            foreach (var o in control.collisionSpheres.bottomSpheres)
            {
                var blockingObj = CollisionDetection.GetCollidingObject(control, o, Vector3.down, 0.1f, ref control.animationProgress.collidingPoint);

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
            foreach (KeyValuePair<GameObject, GameObject> data in frontBlockingObjects)
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
            foreach (KeyValuePair<GameObject, GameObject> data in frontBlockingObjects)
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
            frontBlockingObjects.Clear();
        }

        private bool UpBlockingObjDictionaryIsEmpty()
        {
            if (upBlockingObjects.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool FrontBlockingObjDictionaryIsEmpty()
        {
            if (frontBlockingObjects.Count == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<GameObject> GetFrontBlockingCharacters()
        {
            _frontBlockingCharacters.Clear();

            foreach (KeyValuePair<GameObject,GameObject> data in frontBlockingObjects)
            {
                var c = CharacterManager.Instance.GetCharacter(data.Value.transform.root.gameObject);

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

            foreach(KeyValuePair<GameObject,GameObject> data in frontBlockingObjects)
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