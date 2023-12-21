using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class SubComponentProcessor : MonoBehaviour
    {
        public Dictionary<SubComponents, SubComponent> componentsDictionary = new Dictionary<SubComponents, SubComponent>();
        public CharacterControl control;

        [Space(15)]
        public BlockingObjData blockingObjData;
        [Space(15)]
        public LedgeGrab_Data ledgeGrabData;
        [Space(15)]
        public RagdollData ragdollData;

        private void Awake()
        {
            control = GetComponentInParent<CharacterControl>();
        }
        public void FixedUpdateSubComponents()
        {
            FixedUpdateSubComponent(SubComponents.LEDGECHECKER);
            FixedUpdateSubComponent(SubComponents.RAGDOLL);
            FixedUpdateSubComponent(SubComponents.BLOCKINGOBJECTS);
        }
        public void UpdateSubComponents()
        {
            UpdateSubComponent(SubComponents.MANUALINPUT);
        }

        private void UpdateSubComponent(SubComponents type)
        {
            if (componentsDictionary.ContainsKey(type))
            {
                componentsDictionary[type].OnUpdate();
            }
        }
        private void FixedUpdateSubComponent(SubComponents type)
        {
            if (componentsDictionary.ContainsKey(type))
            {
                componentsDictionary[type].OnFixedUpdate();
            }
        }
    }
}

