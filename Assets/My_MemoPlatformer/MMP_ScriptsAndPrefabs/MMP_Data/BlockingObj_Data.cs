using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [System.Serializable]
    public class BlockingObj_Data
    {
        public int frontBlockingDictionaryCount;
        public int upBlockingDictionaryCount;

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