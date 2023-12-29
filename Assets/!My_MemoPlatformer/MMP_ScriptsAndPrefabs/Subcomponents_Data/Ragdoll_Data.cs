using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [System.Serializable]
    public class Ragdoll_Data
    {
        public bool ragdollTriggered;
        public Collider[] arrBodyParts;
        public FlyingRagdollData flyingRagdollData;

        public delegate Collider GetCollider(string name);
        public delegate void ProcRagdoll(RagdollPushType type);
        public delegate void DoSomething();

        public GetCollider GetBodypart;
        public ProcRagdoll AddForceToDamagedPart;
        public DoSomething ClearExistingVelocity;
    }

    [System.Serializable]
    public class FlyingRagdollData
    {
        public bool isTriggered = false;
        public CharacterControl attacker = null;
    }
}