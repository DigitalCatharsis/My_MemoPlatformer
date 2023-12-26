using System;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    public class PlayerGround_Data
    {
        public GameObject ground;
        public ContactPoint[] BoxColliderContacts;
    }
}

