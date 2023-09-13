using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AnimationProgress : MonoBehaviour
    {
        public List<StateData> currentRunningAbilities = new List<StateData>();

        public bool cameraShaken;
        public List<PoolObjectType> poolObjectList = new List<PoolObjectType>();
        public bool attackTriggered;
        public bool ragdollTriggered;
        public float maxPressTime;

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


        private CharacterControl _control;
        private float _pressTime;

        private void Awake()
        {
            _control = GetComponentInParent<CharacterControl>();
            _pressTime = 0f;
        }

        private void Update()
        {
            if (_control.attack)
            {
                _pressTime += Time.deltaTime;
            }
            else
            {
                _pressTime = 0f;
            }

            if (_pressTime == 0f)
            {
                attackTriggered = false;
            }
            else if (_pressTime > maxPressTime)
            {
                attackTriggered = false;
            }
            else
            {
                attackTriggered = true;
            }
        }

        //private void LateUpdate()
        //{
        //    frameUpdated = false;
        //}

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
    }
}
