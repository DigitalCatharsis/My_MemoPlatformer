using System;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    public class ManualInput_Data
    {
        public delegate bool ReturnBool();
        public ReturnBool DoubleTapUp;
        public ReturnBool DoubleTapDown;
    }
}

