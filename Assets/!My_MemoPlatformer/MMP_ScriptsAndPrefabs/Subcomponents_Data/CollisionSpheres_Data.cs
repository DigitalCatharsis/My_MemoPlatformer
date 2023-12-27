using System;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    public class CollisionSpheres_Data
    {
        public List<GameObject> bottomSpheres;
        public List<GameObject> frontSpheres;
        public List<GameObject> backSpheres;
        public List<GameObject> upSpheres;

        public List<OverlapChecker> frontOverlapCheckers;
        public List<OverlapChecker> allOverlapCheckers;

        public delegate void DoSomething();
        public DoSomething Reposition_FrontSpheres;
        public DoSomething Reposition_BottomSpheres;
        public DoSomething Reposition_BackSpheres;
        public DoSomething Reposition_UpSpheres;
    }
}
