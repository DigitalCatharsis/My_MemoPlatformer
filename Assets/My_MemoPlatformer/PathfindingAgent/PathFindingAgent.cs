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

        private List<Coroutine> moveRoutines = new List<Coroutine>();

        public GameObject startSphere;
        public GameObject endSphere;
        public bool startWalk;

        private void Awake()
        {
            _navMeshAgent= GetComponent<NavMeshAgent>();
        }

        public void GoToTarget()
        {
            _navMeshAgent.enabled = true;
            startSphere.transform.parent = null;
            endSphere.transform.parent = null;
            startWalk = false;

            _navMeshAgent.isStopped = false;

            if (targetPlayableCharacter)
            {
                target = CharacterManager.Instance.GetPlayableCharacter().gameObject;
            }

            _navMeshAgent.SetDestination(target.transform.position);

            if (moveRoutines.Count != 0)
            {
                StopCoroutine(moveRoutines[0]);
                moveRoutines.RemoveAt(0);
            }

            moveRoutines.Add(StartCoroutine(Move()));
        }

        IEnumerator Move()
        {
            while (true)
            {
                if (_navMeshAgent.isOnOffMeshLink)
                {
                    startSphere.transform.position = _navMeshAgent.currentOffMeshLinkData.startPos;
                    endSphere.transform.position = _navMeshAgent.currentOffMeshLinkData.endPos;

                    _navMeshAgent.CompleteOffMeshLink();

                    _navMeshAgent.isStopped = true;
                    startWalk = true;
                    yield break;
                }

                Vector3 dist = transform.position - _navMeshAgent.destination;
                if (Vector3.SqrMagnitude(dist) < 0.5f)
                {
                    startSphere.transform.position = _navMeshAgent.destination;
                    endSphere.transform.position = _navMeshAgent.destination;

                    _navMeshAgent.isStopped = true;
                    startWalk = true;
                    yield break;
                }

                yield return new WaitForEndOfFrame();
            }
        }

    }
}