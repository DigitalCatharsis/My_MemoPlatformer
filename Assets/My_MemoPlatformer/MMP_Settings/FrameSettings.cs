using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "Settings", menuName = "My_MemoPlatformer/Settings/FrameSettings")]

    public class FrameSettings : ScriptableObject
    {
        [Range(0.01f,1f)]
        public float TimeScale;
        public int TargetFPS;

    }
}
