using System;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    public class PlayerAnimation_Data
    {
        public Dictionary<StateData, int> currentRunningAbilities = new Dictionary<StateData, int>();
        public delegate bool bool_type(System.Type type);
        public bool_type IsRunning;
    }

}
