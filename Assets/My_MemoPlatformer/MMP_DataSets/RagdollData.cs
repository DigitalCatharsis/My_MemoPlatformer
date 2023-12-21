using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [System.Serializable]
    public class RagdollData
    {
        public bool ragdollTriggered;
        public List<Collider> bodyParts;
    }
}