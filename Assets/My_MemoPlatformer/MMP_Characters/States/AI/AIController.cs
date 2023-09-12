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
        private Coroutine _aIRoutine;

        public void Start()  //Not Awake (cause of we need it work after onEnable)
        {
            InitializeAI();
        }

        public void InitializeAI()
        {
            if (aIlist.Count == 0)
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

            _aIRoutine = StartCoroutine(_InitAI());
        }

        private void OnEnable()
        {
            if (_aIRoutine != null)
            {
                StopCoroutine(_aIRoutine);
            }
        }
        private IEnumerator _InitAI()
        {
            yield return new WaitForEndOfFrame();

            TriggerAI(initialAl);
        }

        public void TriggerAI(AI_TYPE aiType)
        {
            AISubset next = null;


            foreach (AISubset s in aIlist)
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