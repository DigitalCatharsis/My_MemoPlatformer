using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [System.Serializable]
    public class BlockingObj_Data
    {
        public Vector3 raycastContactPoint = new Vector3();

        public int frontBlockingDictionaryCount;
        public int upBlockingDictionaryCount;

        public float frontBlocking_Distance;
        public float airStompDownBlocking_Distance;
        public float upBlocking_Distance;

        public delegate void DoSomething();
        public delegate bool ReturnBool();
        public delegate List<GameObject> ReturnGameOjbList();

        public DoSomething ClearFrontBlockingObjDic;
        public ReturnBool LeftSideBlocked;
        public ReturnBool RightSideBLocked;
        public ReturnGameOjbList GetFrontBlockingObjList;
        public ReturnGameOjbList GetFrontBlockingCharactersList;
    }
}
