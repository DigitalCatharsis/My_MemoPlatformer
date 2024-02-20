using System;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    public class AIController_Data
    {
        [ShowOnly] public string aiStatus;
        public List<GroundAttack> listGroundAttacks = new List<GroundAttack>();

        public bool doFlyingKick;

        public delegate void GroundAttack(CharacterControl control);
        public Action InitializeAI;
    }
}