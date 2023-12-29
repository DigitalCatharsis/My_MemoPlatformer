using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class SubComponentProcessor : MonoBehaviour
    {
        public SubComponent[] arrSubComponents;
        public CharacterControl control;

        [Space(15)] public BlockingObj_Data blockingObj_Data;
        [Space(15)] public LedgeGrab_Data ledgeGrab_Data;
        [Space(15)] public Ragdoll_Data ragdoll_Data;
        [Space(15)] public ManualInput_Data manualInput_Data;
        [Space(15)] public BoxCollider_Data boxCollider_Data;
        [Space(15)] public VerticalVelocity_Data verticalVelocity_Data;
        [Space(15)] public Damage_Data damage_Data;
        [Space(15)] public MomentumCalculator_Data momentumCalculator_Data;
        [Space(15)] public Rotation_Data rotation_Data;
        [Space(15)] public Jump_Data jump_Data;
        [Space(15)] public CollisionSpheres_Data collisionSpheres_Data;
        [Space(15)] public InstaKill_Data instaKill_Data;
        [Space(15)] public Ground_Data ground_Data;
        [Space(15)] public Attack_Data attack_Data;
        [Space(15)] public Animation_Data animation_Data;

        private void Awake()
        {
            arrSubComponents = new SubComponent[(int)SubComponentType.COUNT];
            control = GetComponentInParent<CharacterControl>();
        }
        public void FixedUpdateSubComponents()
        {
            FixedUpdateSubComponent(SubComponentType.LEDGE_CHECKER);
            FixedUpdateSubComponent(SubComponentType.RAGDOLL);
            FixedUpdateSubComponent(SubComponentType.BLOCKING_OBJECTS);
            FixedUpdateSubComponent(SubComponentType.BOX_COLLIDER_UPDATER);
            FixedUpdateSubComponent(SubComponentType.VERTICAL_VELOCITY);
            FixedUpdateSubComponent(SubComponentType.COLLISION_SPHERES);
            FixedUpdateSubComponent(SubComponentType.INSTA_KILL);
            FixedUpdateSubComponent(SubComponentType.DAMAGE_DETECTOR);
            FixedUpdateSubComponent(SubComponentType.PLAYER_ROTATION);
        }
        public void UpdateSubComponents()
        {
            UpdateSubComponent(SubComponentType.MANUAL_INPUT);
            UpdateSubComponent(SubComponentType.PLAYER_ATTACK);
            UpdateSubComponent(SubComponentType.PLAYER_ANIMATION);
        }

        private void UpdateSubComponent(SubComponentType type)
        {
            if (arrSubComponents[(int)type] != null)
            {
                arrSubComponents[(int)type].OnUpdate();
            }
        }
        private void FixedUpdateSubComponent(SubComponentType type)
        {
            if (arrSubComponents[(int)type] != null)
            {
                arrSubComponents[(int)type].OnFixedUpdate();
            }
        }
    }
}

