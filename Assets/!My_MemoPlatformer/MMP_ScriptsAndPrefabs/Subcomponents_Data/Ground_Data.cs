using System;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    public class Ground_Data
    {
        public GameObject ground;
        public ContactPoint[] BoxColliderContacts;
    }
}

