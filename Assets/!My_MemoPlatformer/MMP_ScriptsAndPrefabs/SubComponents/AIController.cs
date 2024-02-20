using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.AI;
using static My_MemoPlatformer.AIController_Data;

namespace My_MemoPlatformer
{
    public enum AiStatus
    {
        Initializing,
        Idle,
        Walking_To_StartSphere,
        Walking_To_EndSphere,
        Running_To_Point,
        Jumping,
        LedgeClimbing,
        Attacking,
    }

    public enum AI_TYPE
    {
        NONE,
        WALK_AND_JUMP,
    }

    public enum AI_State_Name
    {
        SendPathfindingAgent,
        AI_Attack,

        COUNT,
    }

    public enum PlayerType
    {
        None,
        Player,
        Bot,
    }

    public class AIController : SubComponent
    {
        public AIAttacks _aIAttacks;
        public AIBehavior _aIBehavior;
        public AIConditions _aIConditions;
        public AILogistic _aiLogistic;

        public AIController_Data aIController_Data;
        [SerializeField] private PlayerType _playerType;
        public PathFindingAgent pathfindingAgent;
        public Animator aiAnimator; 
        [Range(0f, 1f)] public float flyingKickProbability;

        private void Start()
        {
            aIController_Data = new AIController_Data
            {
                doFlyingKick = false,
                listGroundAttacks = new List<GroundAttack>(),
                aiStatus = null,
            };

            subComponentProcessor.aIController_Data = aIController_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.PLAYER_ROTATION] = this;
        }
        public override void OnUpdate()
        {
        }

        public override void OnFixedUpdate()
        {
        }
        public void InitializeAI()
        {
            aIController_Data.aiStatus = AiStatus.Initializing.ToString();

            var arr = aiAnimator.GetBehaviours<CharacterState>();
            foreach (var aiState in arr)
            {
                aiState.characterControl = Control;
            }

            aIController_Data.listGroundAttacks.Add(_aIAttacks.NormalGroundAttack);
            aIController_Data.listGroundAttacks.Add(_aIAttacks.ForwardGroundAttack);

            StartCoroutine(_aIAttacks._RandomizeNextAttack());

            if (_playerType == PlayerType.Bot)
            {
                if (Control.navMeshObstacle != null)
                {
                    Control.navMeshObstacle.carving = false;
                }
            }
        }
    }
}