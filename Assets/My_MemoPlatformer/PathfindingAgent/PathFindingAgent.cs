using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace My_MemoPlatformer
{
    public class PathFindingAgent : MonoBehaviour
    {
        public bool targetPlayableCharacter;
        public GameObject target;
        private NavMeshAgent _navMeshAgent;

        public Vector3 startPosition;
        public Vector3 endPosition;

        private Coroutine _move;

        private void Awake()
        {
            _navMeshAgent= GetComponent<NavMeshAgent>();
        }

        public void GoToTarget()
        {
            _navMeshAgent.isStopped = false;

            if (targetPlayableCharacter)
            {
                target = CharacterManager.Instance.GetPlayableCharacter().gameObject;
            }

            _navMeshAgent.SetDestination(target.transform.position);

            if (_move != null)
            {
                StopCoroutine(_move);
            }

            _move = StartCoroutine(_Move());
        }

        IEnumerator _Move()
        {
            while (true)
            {
                if (_navMeshAgent.isOnOffMeshLink)
                {
                    startPosition = transform.position;
                    _navMeshAgent.CompleteOffMeshLink();

                    yield return new WaitForEndOfFrame();
                    endPosition =transform.position;
                    _navMeshAgent.isStopped = true;
                    yield break;
                }

                yield return new WaitForEndOfFrame();
            }
        }

    }
}