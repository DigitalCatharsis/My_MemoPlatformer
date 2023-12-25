using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class SubComponentProcessor : MonoBehaviour
    {
        public Dictionary<SubComponentType, SubComponent> componentsDictionary = new Dictionary<SubComponentType, SubComponent>();
        public CharacterControl control;

        [Space(15)]
        public BlockingObj_Data blockingObjData;
        [Space(15)]
        public LedgeGrab_Data ledgeGrabData;
        [Space(15)]
        public Ragdoll_Data ragdollData;
        [Space(15)]
        public ManualInput_Data manualInput_Data;
        [Space(15)]
        public BoxCollider_Data boxCollider_Data;
        [Space(15)]
        public VerticalVelocity_Data verticalVelocity_Data;

        private void Awake()
        {
            control = GetComponentInParent<CharacterControl>();
        }
        public void FixedUpdateSubComponents()
        {
            FixedUpdateSubComponent(SubComponentType.LEDGECHECKER);
            FixedUpdateSubComponent(SubComponentType.RAGDOLL);
            FixedUpdateSubComponent(SubComponentType.BLOCKINGOBJECTS);
            FixedUpdateSubComponent(SubComponentType.BOX_COLLIDER_UPDATER);
            FixedUpdateSubComponent(SubComponentType.VERTICALVELOCITY_DATA);
        }
        public void UpdateSubComponents()
        {
            UpdateSubComponent(SubComponentType.MANUALINPUT);
        }

        private void UpdateSubComponent(SubComponentType type)
        {
            if (componentsDictionary.ContainsKey(type))
            {
                componentsDictionary[type].OnUpdate();
            }
        }
        private void FixedUpdateSubComponent(SubComponentType type)
        {
            if (componentsDictionary.ContainsKey(type))
            {
                componentsDictionary[type].OnFixedUpdate();
            }
        }
    }
}

