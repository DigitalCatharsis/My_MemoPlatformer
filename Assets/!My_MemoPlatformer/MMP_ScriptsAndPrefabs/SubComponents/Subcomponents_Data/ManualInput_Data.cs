using System;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    public class ManualInput_Data
    {
        public Func<bool> DoubleTapUp;
        public Func<bool> DoubleTapDown;
    }
}

