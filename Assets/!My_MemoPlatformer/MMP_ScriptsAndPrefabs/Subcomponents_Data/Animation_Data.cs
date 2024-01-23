using System;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    public class Animation_Data
    {
        public bool instantTransitionMade;

        [Space(10)][Header("Current")]
        public string currentState;
        public List<string> currentRunningAbilities_PreviewList;

        [Space (10)][Header("Previous")]
        public string previousState;
        public List<string> PreviousRunningAbilities_PreviewList;


        public Dictionary<CharacterAbility, int> currentRunningAbilities_Dictionary = new Dictionary<CharacterAbility, int>();

        public delegate bool bool_type(Type type);
        public bool_type IsRunning;
    }
}
