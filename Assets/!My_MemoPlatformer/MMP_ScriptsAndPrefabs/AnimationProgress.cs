using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AnimationProgress : MonoBehaviour
    {
        public bool cameraShaken;
        public List<PoolObjectType> poolObjectList = new List<PoolObjectType>();

        public MoveForward latestMoveForwardScript;  //latest moveforward script
        public MoveUp latestMoveUpScript;  //latest moveforward script

        [Header("GroundMovement")]
        public bool isIgnoreCharacterTime; //slide beyond character (start ignoring character collider)

        [Header("Colliding Objects")]
        public Dictionary<TriggerDetector, List<Collider>> collidingBodyParts = new Dictionary<TriggerDetector, List<Collider>>(); //key trigger detectors, value - colliding bodyparts which are in contract with trigger detectors
        public Dictionary<TriggerDetector, List<Collider>> collidingWeapons = new Dictionary<TriggerDetector, List<Collider>>(); //key trigger detectors, value - colliding bodyparts which are in contract with trigger detectors

        [Header("Transition")]
        public bool lockTransition;

        [Header("Weapon")]
        public MeleeWeapon holdingWeapon;

        private CharacterControl _control;

        private void Awake()
        {
            _control = GetComponentInParent<CharacterControl>();
        }
        public void NullifyUpVelocity()
        {
            _control.RIGID_BODY.velocity = new Vector3(_control.RIGID_BODY.velocity.x,0f,_control.RIGID_BODY.velocity.z);
        }
        public bool IsFacingAtacker()
        {
            var vec = _control.DAMAGE_DATA.damageTaken.ATTACKER.transform.position -
                _control.transform.position;

            if (vec.z < 0f)
            {
                if (_control.ROTATION_DATA.IsFacingForward())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (vec.z > 0f)
            {
                if (_control.ROTATION_DATA.IsFacingForward())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public bool ForwardIsReversed()
        {
            if (latestMoveForwardScript.moveOnHit)
            {
                if (IsFacingAtacker())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (latestMoveForwardScript.Speed > 0f)
            {
                return false;
            }
            else if (latestMoveForwardScript.Speed < 0f)
            {
                return true;
            }

            return false;
        }

        public bool StateNameContains(string str)
        {
            AnimatorClipInfo[] arr = _control.skinnedMeshAnimator.GetCurrentAnimatorClipInfo(0); //have only one layer which is zero

            foreach (var clipinfo in arr)
            {
                if (clipinfo.clip.name.Contains(str))
                {
                    return true;
                }
            }

            return false;
        }

        public MeleeWeapon GetTouchingWeapon()
        {
            foreach (KeyValuePair<TriggerDetector, List<Collider>> data in collidingWeapons)
            {
                var w = data.Value[0].gameObject.GetComponent<MeleeWeapon>();
                return w;
            }

            return null;
        }
    }
}
