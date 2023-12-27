using System;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public enum MainParameterType
    {
        Move,
        Jump,
        ForceTransition,
        Grounded,
        Attack,
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

    public class HashManager : Singleton<HashManager> 
    {
        public int[] ArrMainParams = new int[(int)MainParameterType.COUNT];
        public int[] ArrCameraParams = new int[(int)CameraTrigger.COUNT];
        public int[] ArrAITransitionParams = new int[(int)AI_Transition.COUNT];

        private void Awake()
        {
            // animation transitions
            for (int i = 0; i < (int)MainParameterType.COUNT; i++)
            {
                ArrMainParams[i] = Animator.StringToHash(((MainParameterType)i).ToString()); //https://docs.unity3d.com/ScriptReference/Animator.StringToHash.html
            }

            // camera transitions
            for (int i = 0; i < (int)CameraTrigger.COUNT; i++)
            {
                ArrCameraParams[i] = Animator.StringToHash(((CameraTrigger)i).ToString()); //https://docs.unity3d.com/ScriptReference/Animator.StringToHash.html
            }

            // ai transitions
            for (int i = 0; i < (int)AI_Transition.COUNT; i++)
            {
                ArrAITransitionParams[i] = Animator.StringToHash(((AI_Transition)i).ToString());
            }
        }
    }
}

