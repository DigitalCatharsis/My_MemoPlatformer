using UnityEngine;

namespace My_MemoPlatformer
{
    public class ManualInput_Data : MonoBehaviour
    {
        public delegate bool ReturnBool();
        public ReturnBool DoubleTapUp;
        public ReturnBool DoubleTapDown;
    }
}

