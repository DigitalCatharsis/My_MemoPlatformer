using System;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    public class PlayerAnimation_Data
    {
        [Header("Transition")]
        public bool instantTransitionMade;
        public bool lockTransition;  //TODO: ���-�� ����� Lock transition (�� �����������). ���� ����� �������� ����, ����� ������ ��� �������.

        [Space(10)][Header("Current")]
        public string currentState;
        public List<string> currentRunningAbilities_PreviewList;

        [Space (10)][Header("Previous")]
        public string previousState;
        public List<string> PreviousRunningAbilities_PreviewList;
        public Dictionary<CharacterAbility, int> currentRunningAbilities_Dictionary = new Dictionary<CharacterAbility, int>();

        public Func<Type,bool> IsRunning;
        public Predicate<string> StateNameContains;

    }
}
