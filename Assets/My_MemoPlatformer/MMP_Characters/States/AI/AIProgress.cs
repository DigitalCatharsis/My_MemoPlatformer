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
            if (CharacterManager.Instance.GetCharacter(_control.aiProgress.pathfindfingAgent.target).DamageDetector_Data.IsDead())
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
                if (_control.PlayerRotation_Data.IsFacingForward())
                {
                    return true;
                }
            }
            else
            {
                if (!_control.PlayerRotation_Data.IsFacingForward())
                {
                    return true;
                }
            }
            
            return false;
        }

        public void RepositionDestination()
        {
            pathfindfingAgent.startSphere.transform.position = pathfindfingAgent.target.transform.position;
            pathfindfingAgent.endSphere.transform.position = pathfindfingAgent.target.transform.position;

        }

        public bool TargetIsOnTheSamePlatform()
        {
            var target = CharacterManager.Instance.GetCharacter(_control.aiProgress.pathfindfingAgent.target);
            if (target.PlayerGround_Data.ground == _control.PlayerGround_Data.ground)
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
            var target = CharacterManager.Instance.GetCharacter(_control.aiProgress.pathfindfingAgent.target);
            if (target.PlayerGround_Data.ground == null)  //not grounded
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
            var vec = _control.transform.position - pathfindfingAgent.startSphere.transform.position;

            return Mathf.Abs(vec.y);
        }
    }
}