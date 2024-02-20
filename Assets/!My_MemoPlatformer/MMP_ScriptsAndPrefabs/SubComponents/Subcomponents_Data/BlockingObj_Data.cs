using System;
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

        [ShowOnlyAttribute] public float frontBlocking_Distance;
        public float airStompDownBlocking_Distance;
        public float upBlocking_Distance;

        public Action ClearFrontBlockingObjDic;
        public Func<bool> LeftSideBlocked;
        public Func<bool> RightSideBLocked;
        public Func<List<GameObject>> GetFrontBlockingObjList;
        public Func<List<GameObject>> GetFrontBlockingCharactersList;
        public Func<GameObject, bool> IsSteppbleObject;
    }
}
