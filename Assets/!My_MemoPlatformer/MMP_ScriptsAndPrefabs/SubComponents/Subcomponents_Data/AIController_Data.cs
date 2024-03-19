using System;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    public class AIController_Data
    {
        [ShowOnly] public string aiStatus;
        public AI_Type aiType;
        public List<GroundAttack> listGroundAttacks = new List<GroundAttack>();
        public CharacterControl blockingCharacter;

        public bool doFlyingKick;
        public float flyingKickProbability;

        public AIAttacks aIAttacks;
        public AIBehaviors aIBehaviors;
        public AIConditions aIConditions;
        public AILogistics aiLogistic;

        public PathFindingAgent pathfindingAgent;
        public Animator aiAnimator;

        public delegate void GroundAttack(CharacterControl control);
        public Action InitializeAI;
    }
}