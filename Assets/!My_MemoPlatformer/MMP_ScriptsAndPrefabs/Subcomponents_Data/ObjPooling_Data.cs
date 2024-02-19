using System;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [System.Serializable]
    public class ObjPooling_Data
    {
        public List<GameObject> dataTypes;
        public List<GameObject> Vfxs;

        public Action<GameObject, Enum> AddToList;
        public Action<GameObject, Enum> RemoveFromList;
    }
}
