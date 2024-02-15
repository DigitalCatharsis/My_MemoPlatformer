using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AIProgress : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float flyingKickProbability;

        public PathFindingAgent pathfindingAgent;
        public CharacterControl blockingCharacter;
        public bool doFlyingKick;

        delegate void GroundAttack(CharacterControl control);
        private List<GroundAttack> _listGroundAttacks = new List<GroundAttack>();
        private int _attackIndex;

        private CharacterControl _control;

        private void Awake()
        {
            _control = this.gameObject.GetComponentInParent<CharacterControl>();
            _listGroundAttacks.Add(NormalGroundAttack);
            _listGroundAttacks.Add(ForwardGroundAttack);

            StartCoroutine(_RandomizeAttack());
        }

        private void NormalGroundAttack(CharacterControl control)
        {
            control.moveRight = false;
            control.moveLeft = false;

            control.ATTACK_DATA.attackTriggered = true;
            control.attack = false;
        }

        void ForwardGroundAttack(CharacterControl control)
        {
            if (control.aiProgress.TargetIsOnRightSide())
            {
                control.moveRight = true;
                control.moveLeft = false;

                PrococessForwardGroundAttack(control);
            }
            else
            {
                control.moveRight = false;
                control.moveLeft = true;

                PrococessForwardGroundAttack(control);
            }
        }
        public bool TargetIsOnRightSide()
        {
            if ((_control.aiProgress.pathfindingAgent.target.transform.position - _control.transform.position).z > 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void PrococessForwardGroundAttack(CharacterControl control)
        {
            if (control.aiProgress.IsFacingTarget() && control.PLAYER_ANIMATION_DATA.IsRunning(typeof(MoveForward)))
            {
                control.ATTACK_DATA.attackTriggered = true;
                control.attack = false;
            }
        }

        public bool IsFacingTarget()
        {
            if ((_control.aiProgress.pathfindingAgent.target.transform.position - _control.transform.position).z > 0f)
            {
                if (_control.ROTATION_DATA.IsFacingForward())
                {
                    return true;
                }
            }
            else
            {
                if (!_control.ROTATION_DATA.IsFacingForward())
                {
                    return true;
                }
            }

            return false;
        }

        IEnumerator _RandomizeAttack()
        {
            while (true)
            {
                _attackIndex = Random.Range(0, _listGroundAttacks.Count);
                yield return new WaitForSeconds(2f);
            }
        }
        public void DoAttack()
        {
            _listGroundAttacks[_attackIndex](_control);
        }

        public bool TargetIsDead()
        {
            if (CharacterManager.Instance.GetCharacter(_control.aiProgress.pathfindingAgent.target).DAMAGE_DATA.IsDead())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TargetIsOnTheSamePlatform()
        {
            var target = CharacterManager.Instance.GetCharacter(_control.aiProgress.pathfindingAgent.target);

            if (target.GROUND_DATA.ground == _control.GROUND_DATA.ground)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public float TargetDistanceToEndSphere()
        {
            return Vector3.SqrMagnitude(
                _control.aiProgress.pathfindingAgent.endSphere.transform.position - _control.aiProgress.pathfindingAgent.target.transform.position);
        }

        public bool TargetIsGrounded()
        {
            var target = CharacterManager.Instance.GetCharacter(_control.aiProgress.pathfindingAgent.target);
            if (target.GROUND_DATA.ground == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void RepositionDestination()
        {
            pathfindingAgent.startSphere.transform.position = pathfindingAgent.target.transform.position;
            pathfindingAgent.endSphere.transform.position = pathfindingAgent.target.transform.position;
        }

        public void SetRandomFlyingKick()
        {
            if (Random.Range(0f, 1f) < flyingKickProbability)
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
            var result = Mathf.Abs((_control.transform.position - pathfindingAgent.startSphere.transform.position).y);

            return result;
        }

        public float GetEndSphereHeight()
        {
            var result = Mathf.Abs((_control.transform.position - pathfindingAgent.endSphere.transform.position).y);

            return result;
        }

        public bool EndSphereIsHigher()
        {
            if (EndSphereIsStraight())
            {
                return false;
            }

            if (pathfindingAgent.endSphere.transform.position.y - pathfindingAgent.startSphere.transform.position.y > 0f)
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

            if (pathfindingAgent.endSphere.transform.position.y - pathfindingAgent.startSphere.transform.position.y > 0f)
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
            if (Mathf.Abs(pathfindingAgent.endSphere.transform.position.y - pathfindingAgent.startSphere.transform.position.y) > 0.01f)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public float AIDistanceToTarget()
        {
            var dist = Vector3.SqrMagnitude(_control.aiProgress.pathfindingAgent.target.transform.position - _control.transform.position);
            return dist;
        }

        public float AIDistanceToStartSphere()
        {
            var dist = Vector3.SqrMagnitude(_control.aiProgress.pathfindingAgent.startSphere.transform.position - _control.transform.position);
            return dist;
        }

        public float AIDistanceToEndSphere()
        {
            var dist = Vector3.SqrMagnitude(_control.aiProgress.pathfindingAgent.endSphere.transform.position - _control.transform.position);
            return dist;
        }
    }
}