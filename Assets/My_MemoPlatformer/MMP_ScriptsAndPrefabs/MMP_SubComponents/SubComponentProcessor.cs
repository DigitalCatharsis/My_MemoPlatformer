using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class SubComponentProcessor : MonoBehaviour
    {
        public Dictionary<SubComponentType, SubComponent> subcomponentsDictionary = new Dictionary<SubComponentType, SubComponent>();
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
        [Space(15)]
        public DamageDetector_Data damageDetector_Data;
        [Space(15)]
        public MomentumCalculator_Data momentumCalculator_Data;
        [Space(15)]
        public PlayerRotation_Data playerRotation_Data;
        [Space(15)]
        public PlayerJump_Data playerJump_Data;
        [Space(15)]
        public CollisionSpheres_Data collisionSpheres_Data;
        [Space(15)]
        public InstaKill_Data instaKill_Data;

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
            FixedUpdateSubComponent(SubComponentType.COLLISION_SPHERES);
            FixedUpdateSubComponent(SubComponentType.INSTAKILL);
        }
        public void UpdateSubComponents()
        {
            UpdateSubComponent(SubComponentType.MANUALINPUT);
            UpdateSubComponent(SubComponentType.DAMAGE_DETECTOR_DATA);
        }

        private void UpdateSubComponent(SubComponentType type)
        {
            if (subcomponentsDictionary.ContainsKey(type))
            {
                subcomponentsDictionary[type].OnUpdate();
            }
        }
        private void FixedUpdateSubComponent(SubComponentType type)
        {
            if (subcomponentsDictionary.ContainsKey(type))
            {
                subcomponentsDictionary[type].OnFixedUpdate();
            }
        }
    }
}

