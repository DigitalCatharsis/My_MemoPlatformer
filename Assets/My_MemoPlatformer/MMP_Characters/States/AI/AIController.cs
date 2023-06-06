using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public enum AI_TYPE
    {
        WALK_AND_JUMP,
        RUN,
    }
    public class AIController : MonoBehaviour
    {
        public List<AISubset> aIlist = new List<AISubset>();
        public AI_TYPE initialAl;

        public void Awake()
        {
            AISubset[] arr = this.gameObject.GetComponentsInChildren<AISubset>();

            foreach (AISubset s in arr)
            {
                if (!aIlist.Contains(s))
                {
                    aIlist.Add(s);
                    s.gameObject.SetActive(false);
                }
            }
        }

        private IEnumerator Start()
        {
             yield return new WaitForEndOfFrame();

            TriggerAI(initialAl);
        }

        public void TriggerAI(AI_TYPE aiType)
        {
            AISubset next = null;


            foreach(AISubset s in aIlist)
            {
                s.gameObject.SetActive(false);

                if (s.aiType == aiType)
                {
                    next = s;
                }
            }

            if (next != null)
            {
                next.gameObject.SetActive(true);
            }
        }

    }
}