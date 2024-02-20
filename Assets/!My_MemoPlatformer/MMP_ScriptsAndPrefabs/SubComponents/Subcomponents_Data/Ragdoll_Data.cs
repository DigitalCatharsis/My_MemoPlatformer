using System;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [System.Serializable]
    public class Ragdoll_Data
    {
        public bool ragdollTriggered;
        public Collider[] arrBodyPartsColliders;
        public FlyingRagdollData flyingRagdollData;

        public Func<string,Collider> GetBodypart;
        public Action<RagdollPushType> AddForceToDamagedPart;
        public Action ClearExistingVelocity;
    }

    [System.Serializable]
    public class FlyingRagdollData
    {
        public bool isTriggered = false;
        public CharacterControl attacker = null;
    }
}