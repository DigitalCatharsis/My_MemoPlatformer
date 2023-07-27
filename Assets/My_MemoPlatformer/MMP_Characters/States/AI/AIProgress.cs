using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AIProgress : MonoBehaviour
    {
        public PathFindingAgent pathfindfingAgent;

        CharacterControl control;

        private void Awake()
        {
            control = this.gameObject.GetComponentInParent<CharacterControl>();
        }
        public float GetDistanceToDestination()
        {
            return Vector3.SqrMagnitude(control.aiProgress.pathfindfingAgent.startSphere.transform.position 
                - control.transform.position); //distance between checkpoint and character
        }
    }
}