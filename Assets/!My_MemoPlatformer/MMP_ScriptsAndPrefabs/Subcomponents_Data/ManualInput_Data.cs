using UnityEngine;

namespace My_MemoPlatformer
{
    public class ManualInput_Data
    {
        public delegate bool ReturnBool();
        public ReturnBool DoubleTapUp;
        public ReturnBool DoubleTapDown;
    }
}

