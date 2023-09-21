using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class HashManager : Singleton<HashManager> 
    { 
        public Dictionary<TransitionParameter, int> dicMainParams = new Dictionary<TransitionParameter, int>();

        private void Awake()
        {
            TransitionParameter[] arr = System.Enum.GetValues(typeof(TransitionParameter)) as TransitionParameter[];

            foreach (TransitionParameter t in arr) 
            {
                dicMainParams.Add(t, Animator.StringToHash(t.ToString()));  //https://docs.unity3d.com/ScriptReference/Animator.StringToHash.html
            }
        }
    }
}

