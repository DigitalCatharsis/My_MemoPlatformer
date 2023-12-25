using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AnimationProgress : MonoBehaviour
    {
        public Dictionary<StateData, int> currentRunningAbilities = new Dictionary<StateData, int>();

        public bool cameraShaken;
        public List<PoolObjectType> poolObjectList = new List<PoolObjectType>();
        public MoveForward latestMoveForwardScript;  //latest moveforward script
        public MoveUp latestMoveUpScript;  //latest moveforward script

        [Header("Attack Button")]
        public bool attackTriggered;
        public bool attackButtonIsReset;

        [Header("GroundMovement")]
        public bool isIgnoreCharacterTime; //slide beyond character (start ignoring character collider)

        [Header("Colliding Objects")]
        public GameObject ground;
        public Dictionary<TriggerDetector, List<Collider>> collidingBodyParts = new Dictionary<TriggerDetector, List<Collider>>(); //key trigger detectors, value - colliding bodyparts which are in contract with trigger detectors
        public Dictionary<TriggerDetector, List<Collider>> collidingWeapons = new Dictionary<TriggerDetector, List<Collider>>(); //key trigger detectors, value - colliding bodyparts which are in contract with trigger detectors

        public Vector3 collidingPoint = new Vector3();

        [Header("Transition")]
        public bool lockTransition;

        [Header("Weapon")]
        public MeleeWeapon HoldingWeapon;

        private CharacterControl _control;

        private void Awake()
        {
            _control = GetComponentInParent<CharacterControl>();
        }

        private void Update()
        {
            if (_control.attack)
            { //dont trigger attack several times
                if (attackButtonIsReset)
                {
                    attackTriggered = true;
                    attackButtonIsReset = false;
                }
            }
            else
            {
                attackButtonIsReset = true;
                attackTriggered = false;
            }

            if (IsRunning(typeof(LockTransition)))
            {
                if (_control.animationProgress.lockTransition)
                {
                    _control.skinnedMeshAnimator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.LockTransition], true);
                }
                else
                {
                    _control.skinnedMeshAnimator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.LockTransition], false);
                }
            }
            else
            {
                _control.skinnedMeshAnimator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.LockTransition], false);
            }
        }
        public void NullifyUpVelocity()
        {
            _control.Rigid_Body.velocity = new Vector3(
                _control.Rigid_Body.velocity.x,
                0f,
                _control.Rigid_Body.velocity.z);
        }
        public bool IsFacingAtacker()
        {
            var vec = _control.DamageDetector_Data.attacker.transform.position - _control.transform.position;

            if (vec.z < 0f)
            {
                if (_control.IsFacingForward())
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
                if (_control.IsFacingForward())
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

            if (latestMoveForwardScript.speed > 0f)
            {
                return false;
            }
            else if (latestMoveForwardScript.speed < 0f)
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

        public bool IsRunning(System.Type type) //ability is running now?
        {
            foreach (KeyValuePair<StateData, int> data in currentRunningAbilities)
            {
                if (data.Key.GetType() == type)
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
