using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace My_MemoPlatformer
{
    public enum StatesList
    {
        //Ground Movement
        Running,
        Walking,

        //Running Transitions
        BackFlip,
        RunningTurn_180,
        ForwardRoll_End,
        ForwardRoll_Start,
        RunningSlide,
        RunToStop,

        //Base
        Idle,
        Block,
        BlockReaction,

        //Ground Attacks
        Uppercut,
        FlyingKick,
        SpinningBackKick,
        BallKick,
        Shoryuken_Prep,
        Shoryuken,
        Shoryuken_DownSmash_1,
        Shoryuken_DownSmash_2,
        JebCrossHookCombo_Jab,
        JumpingPunch,
        JebCrossHookCombo_Hook,
        JebCrossHookCombo_Cross,

        //Damage Reaction
        Head_Hit,
        Zombie_Death,

        //Axe
        Axe_StandingAttack,
        Axe_Idle,
        Axe_Running,
        Axe_Walk,
        Throw_Axe,
        WeaponPickUp,
        WeaponPutDown,

        //Air Movement
        Fall,
        Jump_Normal_Prep,
        Jump_Normal,
        Jump_Normal_Landing,
        Jump_Running,
        Jump_Running_Fall,
        LedgeClimb,
        Hanging_Idle,
        WallSlide,
        WallJump_Prep,
        WallJump,

    }

    public enum AbilitiesList
    {
        Attack,
        Block,
        CheckMovement,
        CheckRunningTurn,
        CheckTurbo,
        CheckTurboAndMovement,
        ForceTransition,
        GroundDetector,
        Idle,
        InstantTransition,
        Jump,
        JumpPrep,
        Landing,
        LockTransition,
        LockTurn,
        MoveForward,
        MoveUp,
        ResetLocalPosition,
        ShakeCamera,
        SpawnObject,
        SwitchAnimator,
        ToggleBoxCollider,
        ToggleGravity,
        TransitionConditionChecker,
        TransitionIndexer,
        Turn180,
        TurnOnRootMotion,
        UpdateBoxCollider,
        WallJump_Prep,
        WallSlide,
        WeaponPickup,
        WeaponPutDown,
        WeaponThrow,
        FallPlatform,
        JumpPlatform,
        SendPathfindingAgent,
        StartWalking,
        COUNT
    }

    public enum MainParameterType
    {
        Move,
        Left,
        Right,
        Up,
        Down,
        Jump,
        ForceTransition,
        Grounded,
        ClickAnimation,
        TransitionIndex,
        Turbo,
        Turn,
        LockTransition,

        COUNT,
    }

    public enum CameraTrigger
    {
        Default,
        Shake,

        COUNT,
    }
    public enum AI_Transition
    {
        Start_Walking,
        Jump_Platform,
        Fall_Platform,

        COUNT,
    }
    public enum MMP_Scenes
    {
        L_CharacterSelect,
        L_Level_Start,
        L_Level_Day,
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

    public enum Hit_Reaction_States
    {
        Head_Hit,
        Zombie_Death,

        COUNT,
    }

    public enum Instant_Transition_States
    {
        Jump_Normal_Landing = 0,
        Hanging_Idle = 2,
        Idle = 4,

        Jump_Normal_Prep = 5,
        Jump_Running = 6,

        COUNT,
    }

    public enum Ledge_Trigger_States
    {
        Jump_Running_Fall,

        // normal jump
        Jump_Normal,
        //Heroic_Fall,

        // running jump
        Running_Jump,
        //Running_Heroic_Fall,
        Jump_Running,

        Fall,
        WallSlide,
        WallJump,

        COUNT,
    }

    public enum Camera_States
    {
        Default,
        Shake,
    }
    public class HashManager : Singleton<HashManager>
    {
        public int[] arrMainParams = new int[(int)MainParameterType.COUNT];
        public int[] arrCameraParams = new int[(int)CameraTrigger.COUNT];
        public int[] arrAITransitionParams = new int[(int)AI_Transition.COUNT];
        public int[] arrAIStateNames = new int[(int)AI_State_Name.COUNT];

        public Dictionary<Hit_Reaction_States, int> dicHitReactionStates =
            new Dictionary<Hit_Reaction_States, int>();

        public int[] arrInstantTransitionStates = new int[(int)Instant_Transition_States.COUNT];
        public int[] arrLedgeTriggerStates = new int[(int)Ledge_Trigger_States.COUNT];

        public int[] arrAllAbilitiesList = new int[(int)AbilitiesList.COUNT];

        public Dictionary<Camera_States, int> dicCameraStates = new Dictionary<Camera_States, int>();

        public StatesHashList statesHashViewer = new StatesHashList();

        private void Awake()
        {
            {
                // animation transitions
                for (int i = 0; i < (int)MainParameterType.COUNT; i++)
                {
                    arrMainParams[i] = Animator.StringToHash(((MainParameterType)i).ToString()); //https://docs.unity3d.com/ScriptReference/Animator.StringToHash.html
                }


                // camera transitions
                for (int i = 0; i < (int)CameraTrigger.COUNT; i++)
                {
                    arrCameraParams[i] = Animator.StringToHash(((CameraTrigger)i).ToString()); //https://docs.unity3d.com/ScriptReference/Animator.StringToHash.html
                }

                // ai transitions
                for (int i = 0; i < (int)AI_Transition.COUNT; i++)
                {
                    arrAITransitionParams[i] = Animator.StringToHash(((AI_Transition)i).ToString());
                }

                // ai states
                for (int i = 0; i < (int)AI_State_Name.COUNT; i++)
                {
                    arrAIStateNames[i] = Animator.StringToHash(((AI_State_Name)i).ToString());
                }

                // hit reaction states
                Hit_Reaction_States[] arrHitReactionStates = System.Enum.GetValues(typeof(Hit_Reaction_States))
                    as Hit_Reaction_States[];

                foreach (var t in arrHitReactionStates)
                {
                    dicHitReactionStates.Add(t, Animator.StringToHash(t.ToString()));
                }


                for (int i = 0; i < arrInstantTransitionStates.Length; i++)
                {
                    arrInstantTransitionStates[i] = Animator.StringToHash(((Instant_Transition_States)i).ToString());
                }


                for (int i = 0; i < arrLedgeTriggerStates.Length; i++)
                {
                    arrLedgeTriggerStates[i] = Animator.StringToHash(((Ledge_Trigger_States)i).ToString());
                }


                for (int i = 0; i < arrAllAbilitiesList.Length; i++)
                {
                    arrAllAbilitiesList[i] = Animator.StringToHash(((AbilitiesList)i).ToString());
                }

                // camera states
                Camera_States[] arrCameraStates = System.Enum.GetValues(typeof(Camera_States))
                    as Camera_States[];

                foreach (Camera_States t in arrCameraStates)
                {
                    dicCameraStates.Add(t, Animator.StringToHash(t.ToString()));
                }
            }

            statesHashViewer.Fill<Camera_States>();
            statesHashViewer.Fill<MainParameterType>();
            statesHashViewer.Fill<CameraTrigger>();
            statesHashViewer.Fill<AI_Transition>();
            statesHashViewer.Fill<AI_State_Name>();
            statesHashViewer.Fill<Instant_Transition_States>();
            statesHashViewer.Fill<Ledge_Trigger_States>();
            statesHashViewer.Fill<AbilitiesList>();
            statesHashViewer.Fill<StatesList>();

        }

        public bool IsStateInCurrent_StateEnum<T>(CharacterControl control, int hash) where T : Enum
        {
            var statesHashViewerList = statesHashViewer.GetSublistByName(typeof(T).ToString());

            for (int i = 0; i < statesHashViewerList.subList.Count; i++)
            {
                if (hash == statesHashViewerList.subList[i].hashedNameValue)
                {
                    if (DebugContainer_Data.Instance.debug_HashManager)
                    {
                        Debug.Log($"{control.gameObject.name} currently is in state : {Enum.GetName(typeof(T), i)}. Current EnumStates_Name {typeof(T)}");
                    }
                    return true;
                }
            }
            if (DebugContainer_Data.Instance.debug_HashManager)
            {
                Debug.Log($"{control.gameObject.name} currently is NOT in StateEnum : {typeof(T)}");
            }
            return false;
        }

        public string GetStateNameByHash<T>(int hash)
        {
            var statesHashViewerList = statesHashViewer.GetSublistByName(typeof(T).ToString());

            for (int i = 0; i < statesHashViewerList.subList.Count; i++)
                if (hash == statesHashViewerList.subList[i].hashedNameValue)
                {
                    return statesHashViewerList.subList[i].stateName;
                }
            return null;
        }

        public string FindStateName_Everywhere_ByHash(int hash)
        {
            foreach (var subList in statesHashViewer.instance)
            {
                foreach (var data in subList.subList)
                {
                    if (data.hashedNameValue == hash)
                    {
                        Debug.Log($"Found hash! State name: {data.stateName}\n stateEnum: {subList.name}");
                        return data.stateName;
                    }
                }
            }

            return null;
        }
    }

    [Serializable]
    public class StatesHashList : Serializable_List<StatesHashData>
    {
        public void Fill<TEnum>() where TEnum : Enum
        {
            var list = new Serializable_SubList<StatesHashData>()
            {
                name = typeof(TEnum).ToString().Replace(typeof(HashManager).Namespace + ".", ""),
                subList = ((TEnum[])Enum.GetValues(typeof(TEnum))).Select(v => new StatesHashData
                {
                    stateName = Enum.GetName(typeof(TEnum), v),
                    hashedNameValue = Animator.StringToHash(v.ToString())
                }).ToList()
            };

            instance.Add(list);
        }
    }

    [Serializable]
    public class StatesHashData
    {
        [SerializeField]
        public string stateName;
        [SerializeField]
        public int hashedNameValue;
    }
}

