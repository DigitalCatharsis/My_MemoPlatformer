using System;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]

    public class Rotation_Data
    {
        public bool lockTurn;
        public float unlockTiming;

        public delegate bool ReturnBool();
        public delegate void DoSomething(bool faceForward);

        public ReturnBool IsFacingForward;
        public DoSomething FaceForward;
    }

}
