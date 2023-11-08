using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class HashManager : Singleton<HashManager> 
    { 
        public Dictionary<TransitionParameter, int> dicMainParams = new Dictionary<TransitionParameter, int>();
        public Dictionary<CameraTrigger, int> dicCameraTriggers = new Dictionary<CameraTrigger, int>();
        public Dictionary<AI_Walk_Transitions, int> dicAITransitions = new Dictionary<AI_Walk_Transitions, int>();

        private void Awake()
        {
            //animation transitions
            TransitionParameter[] arrParams = System.Enum.GetValues(typeof(TransitionParameter)) as TransitionParameter[];

            foreach (TransitionParameter t in arrParams) 
            {
                dicMainParams.Add(t, Animator.StringToHash(t.ToString()));  //https://docs.unity3d.com/ScriptReference/Animator.StringToHash.html
            }

            //animation transitions
            CameraTrigger[] arrCamTransitions = System.Enum.GetValues(typeof(CameraTrigger)) as CameraTrigger[];

            foreach (CameraTrigger t in arrCamTransitions) 
            {
                dicCameraTriggers.Add(t, Animator.StringToHash(t.ToString()));  //https://docs.unity3d.com/ScriptReference/Animator.StringToHash.html
            }

            //animation transitions
            AI_Walk_Transitions[] arrAITransitions = System.Enum.GetValues(typeof(AI_Walk_Transitions)) as AI_Walk_Transitions[];

            foreach (AI_Walk_Transitions t in arrAITransitions) 
            {
                dicAITransitions.Add(t, Animator.StringToHash(t.ToString()));  //https://docs.unity3d.com/ScriptReference/Animator.StringToHash.html
            }
        }
    }
}

