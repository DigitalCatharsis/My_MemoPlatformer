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
        private Coroutine _moveRoutine;
        public GameObject startSphere;
        public GameObject endSphere;
        public bool startWalk;
        public List<Vector3> meshLinks = new List<Vector3>();

        public CharacterControl owner = null;

        private void Awake()
        {
            _navMeshAgent= GetComponent<NavMeshAgent>();
        }

        public void GoToTarget()
        {
            meshLinks.Clear();

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
                    if (meshLinks.Count == 0)
                    {
                        meshLinks.Add(_navMeshAgent.currentOffMeshLinkData.startPos);
                        meshLinks.Add(_navMeshAgent.currentOffMeshLinkData.endPos);
                    }

                    //startSphere.transform.position = _navMeshAgent.currentOffMeshLinkData.startPos;
                    //endSphere.transform.position = _navMeshAgent.currentOffMeshLinkData.endPos;

                    //_navMeshAgent.CompleteOffMeshLink();

                    //_navMeshAgent.isStopped = true;
                    //startWalk = true;
                    //break;
                }

                var dist = transform.position - _navMeshAgent.destination;  //����� ����������� � ��� ������ ����������, � �� control
                if (Vector3.SqrMagnitude(dist) < 0.5f)  
                {
                    if (meshLinks.Count > 0)
                    {
                        startSphere.transform.position = meshLinks[0];
                        endSphere.transform.position = meshLinks[1];
                    }
                    else
                    {
                        startSphere.transform.position = _navMeshAgent.destination;
                        endSphere.transform.position = _navMeshAgent.destination;
                    }

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