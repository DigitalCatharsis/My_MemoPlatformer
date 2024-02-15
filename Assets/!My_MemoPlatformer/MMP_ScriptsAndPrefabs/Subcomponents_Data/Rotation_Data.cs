using System;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    public class Rotation_Data
    {
        public bool lockTurn;
        public float unlockTiming;

        public Func<bool> IsFacingForward;
        public Action<bool> FaceForward;
    }
}
