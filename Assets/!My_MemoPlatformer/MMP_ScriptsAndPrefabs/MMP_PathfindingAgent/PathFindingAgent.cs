using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.AI;

namespace My_MemoPlatformer
{
    //Dont make speed of PA more than 45, or it will be insane sometimes
    public class PathFindingAgent : MonoBehaviour
    {
        public bool targetPlayableCharacter;
        public GameObject target;
        public NavMeshAgent _navMeshAgent;
        private Coroutine _moveRoutine;
        public GameObject startSphere;
        public GameObject endSphere;
        public GameObject testSphere;
        //public bool hasFinishedPathfind;
        public List<Vector3> meshLinks = new List<Vector3>();

        public CharacterControl owner = null; //OLD

        private void Awake()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            startSphere.transform.parent = null;
            endSphere.transform.parent = null;
            testSphere.transform.parent = null;
        }
        public IEnumerator ReinitAndSendPA(CharacterControl owner)
        {
            owner.navMeshObstacle.carving = false; //to prevent bug when carving forbids agent to move
            meshLinks.Clear();
            _navMeshAgent.Warp(owner.transform.position + (Vector3.up * 0.5f));
            _navMeshAgent.enabled = true;
            _navMeshAgent.isStopped = false;
            var cycleCounter = -1; //temp

            while (true)
            {
                cycleCounter++;
                _navMeshAgent.SetDestination(target.transform.position);
                //StartCoroutine(SimulateWaypoints(cycleCounter));

                if (_navMeshAgent.isOnOffMeshLink && meshLinks.Count == 0)
                {
                    meshLinks.Add(_navMeshAgent.currentOffMeshLinkData.startPos);
                    meshLinks.Add(_navMeshAgent.currentOffMeshLinkData.endPos);
                }

                // Вывод каждого узла пути в консоль

                var distanceToDestination = transform.position - _navMeshAgent.destination;  //между навигатором и его точкой назначения, а не control
                testSphere.transform.position = _navMeshAgent.destination;
                if (Vector3.SqrMagnitude(distanceToDestination) < 0.5f)
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
                    owner.navMeshObstacle.carving = true;
                    yield break;
                }
                //ColorDebugLog.Log($"{_navMeshAgent.CalculatePath(target.transform.position, new NavMeshPath())}", System.Drawing.KnownColor.RosyBrown);
                //Debug.Log("Restart of OnDestinationCheck_Routine");
                yield return new WaitForSeconds(0.01f);   //DONT CHANGE OR IT WILL GET THROUGH PUCKIN MESH LINKS WAAAAAGHHHHHH!!!!!!!
            }
        }
        private void OnDestroy()
        {
            if (!this.gameObject.scene.isLoaded) return;
            CorutinesManager.Instance.RemoveValueFromDictionary(this.gameObject, nameof(ReinitAndSendPA));
            CorutinesManager.Instance.RemoveKeyFromDictionary(this.gameObject);
        }

        public void ReinitAgent_And_CheckDestination()
        {
        }

        private IEnumerator SimulateWaypoints(int cycleCounter)
        {
            var spheresArray = new List<GameObject>();

            NavMeshPath path = new NavMeshPath();
            _navMeshAgent.CalculatePath(target.transform.position, path);
            var i = 0;
            foreach (Vector3 waypoint in path.corners)
            {
                i++;
                var testSphere = Instantiate(Resources.Load<GameObject>("TestSphere"));
                spheresArray.Add(testSphere);
                testSphere.transform.position = waypoint;
                ColorDebugLog.Log("Cycle " + cycleCounter.ToString(), System.Drawing.KnownColor.Aquamarine);
                ColorDebugLog.Log("Waypoint " + i++ + " " + waypoint.ToString(), System.Drawing.KnownColor.Aquamarine);
            }
            yield return new WaitForSeconds(0.3f);
            foreach (var sphere in spheresArray)
            {
                Destroy(sphere.gameObject);
            }

            yield break;
        }
    }
}