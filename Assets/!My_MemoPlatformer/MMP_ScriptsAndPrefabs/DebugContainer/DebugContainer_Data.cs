using System;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    //TODO: прицепить в инспектор
    public class DebugContainer_Data : Singleton<DebugContainer_Data>
    {
        public bool debug_CameraState;
        public bool debug_Attack;
        public bool debug_Instakill;
        public bool debug_Ragdoll;
        public bool debug_MoveForward;
        public bool debug_Jump;
        public bool debug_SpawnObjects;
        public bool debug_InputManager;
        public bool debug_TransitionTiming;
        public bool debug_TriggerDetector;
        public bool debug_WallOverlappingStatus;
        public bool debug_AI;
        public bool debug_HashManager;
        public bool debug_Colliders;

        public bool displaySpheresAndColliders;
        public bool displayLedgeCheckers;
    }
}

