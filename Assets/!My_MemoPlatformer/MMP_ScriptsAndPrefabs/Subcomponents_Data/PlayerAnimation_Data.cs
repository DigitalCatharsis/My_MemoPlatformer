using System;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    public class PlayerAnimation_Data
    {
        public Dictionary<CharacterAbility, int> currentRunningAbilities = new Dictionary<CharacterAbility, int>();
        public delegate bool bool_type(System.Type type);
        public bool_type IsRunning;
    }

}
