using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
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
        [Header("Data")]
        public AIController_Data aIController_Data;

        [Header("Method Classes Setup")]
        [SerializeField] private AIAttacks _aIAttacks;
        [SerializeField] private AIBehavior _aIBehavior;
        [SerializeField] private AIConditions _aIConditions;
        [SerializeField] private AILogistic _aiLogistic;

        [Header("Parameters Setup")][Space(10)]
        [SerializeField] private Animator _aiAnimator;
        [SerializeField] private PlayerType _playerType;
        [Range(0f, 1f)][SerializeField] private float _flyingKickProbability;

        private bool aiIsInitialized = false;

        private void Start()
        {
            aIController_Data = new AIController_Data
            {
                aIAttacks = null,
                aIBehavior = null,
                aIConditions = null,
                aiLogistic = null,

                listGroundAttacks = new List<GroundAttack>(),
                doFlyingKick = false,
                aiStatus = null,
                aiAnimator = null,
                pathfindingAgent = null,
                playerType = PlayerType.None,
                blockingCharacter = null,

                InitializeAI = InitializeAI,
            };

            Control.InitCharactersStates(_aiAnimator);

            subComponentProcessor.aIController_Data = aIController_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.AI_CONTROLLER_DATA] = this;
        }
        public override void OnUpdate()
        {
        }

        public override void OnFixedUpdate()
        {
            if (_playerType == PlayerType.Bot && aiIsInitialized == false)
            {
                InitializeAI();
            }
        }
        public void InitializeAI() //TODO: Check all calls
        {
            aIController_Data.aiStatus = AiStatus.Initializing.ToString();
            aIController_Data.playerType = PlayerType.Bot;

            aIController_Data.aIAttacks = _aIAttacks;
            aIController_Data.aIBehavior = _aIBehavior;
            aIController_Data.aIConditions = _aIConditions;
            aIController_Data.aiLogistic = _aiLogistic;

            aIController_Data.aiAnimator = _aiAnimator;
            aIController_Data.flyingKickProbability = _flyingKickProbability;

            aIController_Data.listGroundAttacks = new List<GroundAttack>
            {
                _aIAttacks.NormalGroundAttack,
                _aIAttacks.ForwardGroundAttack
            };

            StartCoroutine(_aIAttacks._RandomizeNextAttack());

            if (_playerType == PlayerType.Bot)
            {
                if (Control.navMeshObstacle != null)
                {
                    Control.navMeshObstacle.carving = false;
                }
            }

            aiIsInitialized = true;
        }
    }
}