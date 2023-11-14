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
        public bool doFlyingKick;

        private void Awake()
        {
            _control = this.gameObject.GetComponentInParent<CharacterControl>();
        }
        public float AI_DistanceToStartSphere()
        {
            return Vector3.SqrMagnitude(_control.aiProgress.pathfindfingAgent.startSphere.transform.position
                - _control.transform.position); //distance between checkpoint and character
        }
        public float AI_DistanceToEndSphere()
        {
            return Vector3.SqrMagnitude(_control.aiProgress.pathfindfingAgent.endSphere.transform.position
                - _control.transform.position); //distance between checkpoint and character
        }

        public float AIDistanceToTarget()
        {
            return Vector3.SqrMagnitude(_control.aiProgress.pathfindfingAgent.target.transform.position
                - _control.transform.position);
        }
        public float TargetDistanceToEndSphere()
        {
            return Vector3.SqrMagnitude(_control.aiProgress.pathfindfingAgent.endSphere.transform.position
                - _control.aiProgress.pathfindfingAgent.target.transform.position);
        }

        public bool TargetIsDead()
        {
            if (CharacterManager.Instance.GetCharacter(_control.aiProgress.pathfindfingAgent.target).damageDetector.damageTaken > 0)  //not grounded
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TargetIsOnRightSide()
        {
            if ((_control.aiProgress.pathfindfingAgent.target.transform.position - _control.transform.position).z > 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsFacingTarget()
        {
            if ((_control.aiProgress.pathfindfingAgent.target.transform.position - _control.transform.position).z > 0f)
            {
                if (_control.IsFacingForward())
                {
                    return true;
                }
            }
            else
            {
                if (!_control.IsFacingForward())
                {
                    return true;
                }
            }
            
            return false;
        }

        public bool TargetIsOnTheSamePlatform()
        {
            if (CharacterManager.Instance.GetCharacter(_control.aiProgress.pathfindfingAgent.target).animationProgress.ground == _control.animationProgress.ground)
            {
                return true;
            }
            else
            {
                return false;
            }
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

        public void SetRandomFlyingKick()
        {
            if (Random.Range(0f, 1f) < 0.3f) //30% chance
            {
                doFlyingKick = true;
            }
            else
            {
                doFlyingKick = false;
            }
        }

        public float GetStartSphereHeight()
        {
            Vector3 vec = _control.transform.position - pathfindfingAgent.startSphere.transform.position;

            return Mathf.Abs(vec.y);
        }
    }
}