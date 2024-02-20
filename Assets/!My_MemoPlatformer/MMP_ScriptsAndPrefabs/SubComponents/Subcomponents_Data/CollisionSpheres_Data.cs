using System;
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

        public Func<OverlapChecker, bool> IsFrontSphereContainsOverlapChecker;
        public Action Reposition_FrontSpheres;
        public Action Reposition_BottomSpheres;
        public Action Reposition_BackSpheres;
        public Action Reposition_UpSpheres;
    }
}
