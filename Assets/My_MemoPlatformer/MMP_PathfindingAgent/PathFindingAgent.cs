using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace My_MemoPlatformer
{
    public class PathFindingAgent : MonoBehaviour
    {
        public bool targetPlayableCharacter;
        public GameObject target;
        private NavMeshAgent _navMeshAgent;
        private Coroutine _moveRoutine;

        public GameObject startSphere;
        public GameObject endSphere;
        public bool startWalk;

        public CharacterControl owner = null;

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

            _moveRoutine = StartCoroutine(Move());
        }

        private void OnEnable()
        {
            if (_moveRoutine != null)
            {
                StopCoroutine(_moveRoutine);
            }
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
                    break;
                }

                Vector3 dist = transform.position - _navMeshAgent.destination;  //между навигатором и его точкой назначения, а не control
                if (Vector3.SqrMagnitude(dist) < 0.5f)  
                {                    
                    startSphere.transform.position = _navMeshAgent.destination;
                    endSphere.transform.position = _navMeshAgent.destination;

                    _navMeshAgent.isStopped = true;
                    startWalk = true;
                    break;
                }

                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(0.5f);

            owner.navMeshObstacle.carving = true;
        }

    }
}