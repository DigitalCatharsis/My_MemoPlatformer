using System;
using System.Collections.Generic;

namespace My_MemoPlatformer
{
    [Serializable]
    public class Animation_Data
    {
        public bool instantTransitionMade;
        public Dictionary<CharacterAbility, int> currentRunningAbilities = new Dictionary<CharacterAbility, int>();
        public delegate bool bool_type(System.Type type);
        public bool_type IsRunning;
    }

}
