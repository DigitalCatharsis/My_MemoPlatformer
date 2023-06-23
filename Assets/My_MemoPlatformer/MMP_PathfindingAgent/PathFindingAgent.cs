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

            if (moveRoutines.Count != 0)
            {
                if (moveRoutines[0] != null) 
                {
                StopCoroutine(moveRoutines[0]);                
                }

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
                    owner.navMeshObstacle.carving = true;

                    startSphere.transform.position = _navMeshAgent.currentOffMeshLinkData.startPos;
                    endSphere.transform.position = _navMeshAgent.currentOffMeshLinkData.endPos;

                    _navMeshAgent.CompleteOffMeshLink();

                    _navMeshAgent.isStopped = true;
                    startWalk = true;
                    yield break;
                }

                Vector3 dist = transform.position - _navMeshAgent.destination;  //между навигатором и его точкой назначения, а не control
                if (Vector3.SqrMagnitude(dist) < 0.5f)  
                {
                    if (Vector3.SqrMagnitude(owner.transform.position - _navMeshAgent.destination) > 1f)  //control и точкой назначения
                    {
                        owner.navMeshObstacle.carving = true; //исправлял баг, где carving мешал движению навигатора. Возвращаю carving Обратно
                    }
                    
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