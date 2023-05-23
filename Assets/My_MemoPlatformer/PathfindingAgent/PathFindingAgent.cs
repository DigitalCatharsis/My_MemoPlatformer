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

        public GameObject StartSphere;
        public GameObject EndSphere;

        private void Awake()
        {
            _navMeshAgent= GetComponent<NavMeshAgent>();
        }

        public void GoToTarget()
        {
            StartSphere.transform.parent = null;
            EndSphere.transform.parent = null;

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

            _move = StartCoroutine(Move());
        }

        IEnumerator Move()
        {
            while (true)
            {
                if (_navMeshAgent.isOnOffMeshLink)
                {
                    startPosition = transform.position;
                    StartSphere.transform.position = transform.position;
                    _navMeshAgent.CompleteOffMeshLink();

                    yield return new WaitForEndOfFrame();
                    endPosition = transform.position;
                    EndSphere.transform.position = transform.position;
                    _navMeshAgent.isStopped = true;
                    yield break;
                }

                Vector3 dist = transform.position - _navMeshAgent.destination;
                if (Vector3.SqrMagnitude(dist) < 0.5f)
                {
                    startPosition =transform.position;
                    endPosition = transform.position;
                    _navMeshAgent.isStopped = true;
                    yield break;
                }

                yield return new WaitForEndOfFrame();
            }
        }

    }
}