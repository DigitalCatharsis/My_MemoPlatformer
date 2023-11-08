using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AnimationProgress : MonoBehaviour
    {
        public Dictionary<StateData, int> currentRunningAbilities = new Dictionary<StateData, int>();

        public bool cameraShaken;
        public List<PoolObjectType> poolObjectList = new List<PoolObjectType>();
        public bool ragdollTriggered;
        public MoveForward latestMoveForward;

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
        public Vector3 maxFallVelocity;
        public bool cancelPull;
        public bool canWallJump;
        public bool checkWallBlock;

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
        public GameObject attackingPart;

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

        public bool IsRunning(System.Type type) //ability is running now?
        {
           foreach(KeyValuePair<StateData,int> data in currentRunningAbilities)
            {
                if(data.Key.GetType() == type)
                {
                    return true;
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
