using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public enum AI_TYPE
    {
        NONE,
        WALK_AND_JUMP,
    }
    public class AIController : MonoBehaviour
    {
        public AI_TYPE initialAl;

        private List<AISubset> _aIlist = new List<AISubset>();
        private Coroutine _aIRoutine;
        private Vector3 _targetDir = new Vector3();
        private CharacterControl _control;

        private void Awake()
        {
            _control = this.gameObject.GetComponentInParent<CharacterControl>();
        }

        private void Start()  //Not Awake (cause of we need it work after onEnable)
        {
            InitializeAI();
        }

        public void InitializeAI()
        {
            if (_aIlist.Count == 0)
            {
                AISubset[] arr = this.gameObject.GetComponentsInChildren<AISubset>();

                foreach (AISubset s in arr)
                {
                    if (!_aIlist.Contains(s))
                    {
                        _aIlist.Add(s);
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

            TriggerAI(initialAl);  //trigger each subset
        }

        public void TriggerAI(AI_TYPE aiType)
        {
            AISubset next = null;

            foreach (AISubset s in _aIlist)
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

        public void WalkStraightToTheStartSphere()
        {
            _targetDir = _control.aiProgress.pathfindfingAgent.startSphere.transform.position - _control.transform.position;

            if (_targetDir.z > 0f)
            {
                _control.moveLeft = false;
                _control.moveRight = true;
            }
            else
            {
                _control.moveLeft = true;
                _control.moveRight = false;
            }
        }
        public void WalkStraightToTheEndSphere()
        {
            _targetDir = _control.aiProgress.pathfindfingAgent.endSphere.transform.position
                - _control.transform.position;

            if (_targetDir.z > 0f)
            {
                _control.moveLeft = false;
                _control.moveRight = true;
            }
            else
            {
                _control.moveLeft = true;
                _control.moveRight = false;
            }
        }

    }
}