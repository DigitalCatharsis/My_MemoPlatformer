using System;
using System.Collections.Generic;
using UnityEngine;
using static My_MemoPlatformer.AIController_Data;

namespace My_MemoPlatformer
{
    public enum Ai_Status
    {
        InitializingAI,
        Restarting_AI,
        Starting_AI,
        Starting_To_Walk,
        Idle,
        Moving_To_StartSphere,
        Moving_To_EndSphere,
        Running_To_Point,
        Jumping,
        LedgeClimbing,
        Attacking,
        Triggering_AI_State,
        Sending_Pathfinding_Agent,
        ResetingPASpheresPosition,
        StopingCharacter,
        StartingAiProcessor,
        RestartingProcessorCycle,
    }
    public enum AI_State_Name
    {
        SendPathfindingAgent,
        AI_Attack,

        COUNT,
    }

    public enum AI_Type
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
        [SerializeField] private AIBehaviors _aIBehaviors;
        [SerializeField] private AIConditions _aIConditions;

        [Header("Parameters Setup")]
        [Space(10)]
        [SerializeField] private AILogistics _aiLogistic;
        [SerializeField] private Animator _aiAnimator;
        [Range(0f, 1f)][SerializeField] private float _flyingKickProbability;

        public override void OnComponentEnabled()
        {
            control.InitCharactersStates(_aiAnimator);

            aIController_Data = new AIController_Data
            {
                aIAttacks = _aIAttacks,
                aIBehaviors = _aIBehaviors,
                aIConditions = _aIConditions,
                aiLogistic = _aiLogistic,

                listGroundAttacks = new List<GroundAttack>(),
                doFlyingKick = false,
                aiStatus = null,
                aiAnimator = _aiAnimator,
                pathfindingAgent = null,
                aiType = control.aiType,
                blockingCharacter = null,
                flyingKickProbability = _flyingKickProbability,

            InitializeAI = InitializeAI,
            };

            if (aIController_Data.aiType != AI_Type.Player)
            {
                InitializeAI();
            }
            subComponentProcessor.aIController_Data = aIController_Data;
        }
        public override void OnUpdate()
        {
        }

        public override void OnFixedUpdate()
        {
            if (control.DAMAGE_DATA.IsDead == null)
            {
                return;
            }

            if (control.DAMAGE_DATA.IsDead())
            {
                OnCharacterDies();
            }
        }

        public void InitializeAI() //TODO: Check all calls
        {
            aIController_Data.aiStatus = Ai_Status.InitializingAI.ToString();
            aIController_Data.aiType = AI_Type.Bot;

            aIController_Data.listGroundAttacks = new List<GroundAttack>
            {
                _aIAttacks.NormalGroundAttack,
                _aIAttacks.ForwardGroundAttack
            };

            if (aIController_Data.pathfindingAgent == null)
            {
                aIController_Data.pathfindingAgent = Instantiate(Resources.Load("PathfindingAgent", typeof(GameObject)) as GameObject).GetComponent<PathFindingAgent>();
            }

            //if (control.aiType == AI_Type.Bot)
            //{
            //    if (control.navMeshObstacle != null)
            //    {
            //        control.navMeshObstacle.carving = false;
            //    }
            //}
        }

        //TODO: replace and parse. Happens every update, have to fix
        private void OnCharacterDies()
        {
            if (subComponentProcessor.gameObject.activeSelf && aIController_Data.aiType != AI_Type.Player)
            {
                _aiAnimator.enabled = false;
                Destroy(aIController_Data.pathfindingAgent.startSphere.gameObject);
                Destroy(aIController_Data.pathfindingAgent.endSphere.gameObject);
                Destroy(aIController_Data.pathfindingAgent.gameObject);
                subComponentProcessor.gameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
}