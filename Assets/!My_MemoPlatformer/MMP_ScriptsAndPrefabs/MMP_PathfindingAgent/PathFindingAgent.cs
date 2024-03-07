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
        public NavMeshAgent _navMeshAgent;
        private Coroutine _moveRoutine;
        public GameObject startSphere;
        public GameObject endSphere;
        public bool hasFinishedPathfind;
        public List<Vector3> meshLinks = new List<Vector3>();

        public CharacterControl owner = null; //OLD

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            startSphere.transform.parent = null;
            endSphere.transform.parent = null;
        }

        public void ProocedPathfindingAgent(CharacterControl owner)
        {
            //StopCoroutine(OnDestinationCheck_Routine(owner));
            owner.navMeshObstacle.carving = false; //to prevent bug when carving forbids agent to move

            this.transform.position = owner.transform.position + (Vector3.up * 0.5f);
            _navMeshAgent.enabled = true;
            hasFinishedPathfind = false;
            _navMeshAgent.SetDestination(target.transform.position);

            StartCoroutine(OnDestinationCheck_Routine(owner));
        }
        private IEnumerator OnDestinationCheck_Routine(CharacterControl owner)
        {
            meshLinks.Clear();
            while (true)
            {
                if (_navMeshAgent.isOnOffMeshLink)
                {
                    if (meshLinks.Count == 0)
                    {
                        meshLinks.Add(_navMeshAgent.currentOffMeshLinkData.startPos);
                        meshLinks.Add(_navMeshAgent.currentOffMeshLinkData.endPos);
                    }
                }

                var dist = transform.position - _navMeshAgent.destination;  //между навигатором и его точкой назначения, а не control
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
                    hasFinishedPathfind = true;
                }

                yield return new WaitForEndOfFrame();
                owner.navMeshObstacle.carving = true;
            }

        }

        public void ReinitAgent_And_CheckDestination()
        {
            if (_moveRoutine != null)
            {
                StopCoroutine(_moveRoutine);
                CorutinesManager.Instance.RemoveValueFromDictionary(this.gameObject, nameof(OnDestinationCheck_Routine));
            }

            meshLinks.Clear();

            _navMeshAgent.isStopped = false;
            _navMeshAgent.enabled = true;
            startSphere.transform.parent = null;
            endSphere.transform.parent = null;
            hasFinishedPathfind = false;

            _navMeshAgent.isStopped = false;

            if (targetPlayableCharacter)
            {
                target = CharacterManager.Instance.GetPlayableCharacter().gameObject;
            }

            _navMeshAgent.SetDestination(target.transform.position);
            _moveRoutine = StartCoroutine(OnDestinationCheck_Routine(owner));
        }

        //private void OnEnable()
        //{
        //    if (_moveRoutine != null)
        //    {
        //        StopCoroutine(_moveRoutine);
        //    }
        //}

        private void OnDestroy()
        {
            if (!this.gameObject.scene.isLoaded) return;

            StopCoroutine(_moveRoutine);
            CorutinesManager.Instance.RemoveValueFromDictionary(this.gameObject, nameof(OnDestinationCheck_Routine));
            CorutinesManager.Instance.RemoveKeyFromDictionary(this.gameObject);
        }

    }
}