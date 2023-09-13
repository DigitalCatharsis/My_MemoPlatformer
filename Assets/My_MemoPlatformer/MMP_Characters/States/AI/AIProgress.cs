using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AIProgress : MonoBehaviour
    {
        public PathFindingAgent pathfindfingAgent;
        public CharacterControl blockingCharacter;

        private CharacterControl _control;

        private void Awake()
        {
            _control = this.gameObject.GetComponentInParent<CharacterControl>();
        }
        public float GetDistanceToStartSphere()
        {
            return Vector3.SqrMagnitude(_control.aiProgress.pathfindfingAgent.startSphere.transform.position 
                - _control.transform.position); //distance between checkpoint and character
        }
        public float AI_DistanceToEndSphere()
        {
            return Vector3.SqrMagnitude(_control.aiProgress.pathfindfingAgent.endSphere.transform.position 
                - _control.transform.position); //distance between checkpoint and character
        }
        public float TargetDistanceToEndSphere()
        {
            return Vector3.SqrMagnitude(_control.aiProgress.pathfindfingAgent.endSphere.transform.position 
                - _control.aiProgress.pathfindfingAgent.target.transform.position);
        }

        public bool TargetIsGrounded()
        {
            if (CharacterManager.Instance.GetCharacter(_control.aiProgress.pathfindfingAgent.target).animationProgress.ground == null)  //not grounded
            {
                return false;
            }
            else
            {
            return true;
            }
        }

        public bool EndSphereIsHigher()
        {
            if (EndSphereIsStraight())
            {
                return false;
            }
            if (pathfindfingAgent.endSphere.transform.position.y - pathfindfingAgent.startSphere.transform.position.y > 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool EndSphereIsLower()
        {
            if (EndSphereIsStraight())
            {
                return false;
            }

            if (pathfindfingAgent.endSphere.transform.position.y - pathfindfingAgent.startSphere.transform.position.y > 0f)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool EndSphereIsStraight()
        {
            if (Mathf.Abs(pathfindfingAgent.endSphere.transform.position.y - pathfindfingAgent.startSphere.transform.position.y) > 0.01f)
            {
                return false;
            }
            else 
            {
                return true;
            }
        }
    }
}