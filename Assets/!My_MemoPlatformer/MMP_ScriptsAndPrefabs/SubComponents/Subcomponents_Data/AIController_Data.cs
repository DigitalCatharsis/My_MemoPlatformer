using System;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    public class AIController_Data
    {
        [ShowOnly] public string aiStatus;
        public PlayerType playerType;
        public List<GroundAttack> listGroundAttacks = new List<GroundAttack>();
        public CharacterControl blockingCharacter;

        public bool doFlyingKick;
        public float flyingKickProbability;

        public AIAttacks aIAttacks;
        public AIBehavior aIBehavior;
        public AIConditions aIConditions;
        public AILogistic aiLogistic;

        public PathFindingAgent pathfindingAgent;
        public Animator aiAnimator;

        public delegate void GroundAttack(CharacterControl control);
        public Action InitializeAI;
    }
}