using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [System.Serializable]
    public class LedgeGrab_Data
    {
        public bool isGrabbingLedge;
        public List<Collider> upBlockingObjects;

        public delegate void DoSomething();
        public DoSomething DisableLedgeColliders;
    }
}
