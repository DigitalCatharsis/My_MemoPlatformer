using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AnimationProgress : MonoBehaviour
    {
        public List<StateData> currentRunningAbilities = new List<StateData>();

        public bool cameraShaken;
        public List<PoolObjectType> poolObjectList = new List<PoolObjectType>();
        public bool ragdollTriggered;

        [Header("Attack Button")]
        public bool attackTriggered;
        public bool attackButtonIsReset;


        [Header("GroundMovement")]
        public bool disAllowEarlyTurn;
        public bool lockDirectionNextState;
        public bool isLanding;

        [Header("Colliding Objects")]
        public GameObject ground;
        public GameObject blockingObj;

        [Header("AirControl")]
        public bool jumped;
        public float airMomentum;
        //public bool frameUpdated;
        public bool cancelPull;

        [Header("UpdateBoxCollider")]
        public bool updatingBoxCollider;
        public bool updatingSpheres;
        public Vector3 targetSize;
        public float sizeSpeed;
        public Vector3 targetCenter;
        public float centerSpeed;

        [Header("Damage Info")]
        public Attack attack;
        public CharacterControl attacker;
        public TriggerDetector damagedTrigger;

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
        }

        public bool IsRunning(System.Type type, StateData self) //ability is running now?
        {
            for (int i = 0; i < currentRunningAbilities.Count; i++)
            {
                if (type == currentRunningAbilities[i].GetType())
                {
                    if (currentRunningAbilities[i] == self)
                    {
                        return false;
                    }
                    else
                    {
                        //Debug.Log(type.ToString() + " is already running");
                        return true;
                    }                    
                }
            }
            return false;
        }

        public bool RightSideIsBlocked()
        {
            if (blockingObj == null)
            {
                return false;
            }

            if ((blockingObj.transform.position - _control.transform.position).z > 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool LeftSideIsBlocked()
        {
            if (blockingObj == null)
            {
                return false;
            }

            if ((blockingObj.transform.position - _control.transform.position).z < 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
