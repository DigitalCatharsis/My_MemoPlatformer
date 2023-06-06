using My_MemoPlatformer;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;

namespace My_MemoPlatformer
{

    public class AnimationProgress : MonoBehaviour
    {
        public bool isJumped;
        public bool cameraShaken = false;
        public List<PoolObjectType> poolObjectList = new List<PoolObjectType>();
        public bool attackTriggered;
        public float maxPressTime;
        public bool disAllowEarlyTurn;
        public float airMomentum;

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

            if (_pressTime == 0f )
            {
                attackTriggered = false;
            }
            else if (_pressTime> maxPressTime)
            {
                attackTriggered = false;
            }
            else
            {
                attackTriggered = true;
            }

        }
    }
}