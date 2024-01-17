using System;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    public class CollisionSpheres_Data
    {
        public GameObject[] bottomSpheres;
        public GameObject[] frontSpheres;
        public GameObject[] backSpheres;
        public GameObject[] upSpheres;

        public OverlapChecker[] frontOverlapCheckers; 
        public OverlapChecker[] allOverlapCheckers; 

        public delegate bool ReturnBool(OverlapChecker checker);

        public ReturnBool IsFrontSphereContainsOverlapChecker;

        public delegate void DoSomething();

        public DoSomething Reposition_FrontSpheres;
        public DoSomething Reposition_BottomSpheres;
        public DoSomething Reposition_BackSpheres;
        public DoSomething Reposition_UpSpheres;
    }
}
