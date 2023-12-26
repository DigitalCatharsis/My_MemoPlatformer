using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [System.Serializable]
    public class Ragdoll_Data
    {
        public bool ragdollTriggered;
        public List<Collider> bodyParts;

        public delegate Collider GetCollider(string name);
        public delegate void DoSomething(bool value);

        public GetCollider GetBodypart;
        public DoSomething AddForceToDamagedPart;
    }
}