using System;
using UnityEngine;

namespace My_MemoPlatformer
{
    //TODO: прицепить в инспектор
    public class DebugContainer : Singleton<DebugContainer>
    {
        public bool debug_CameraState;
        public bool debug_Attack;
        public bool debug_Instakill;
        public bool debug_Ragdoll;
        public bool debug_MoveForward;
        public bool debug_Jump;
        public bool debug_SpawnObjects;
        public bool debug_InputManager;
    }
}

