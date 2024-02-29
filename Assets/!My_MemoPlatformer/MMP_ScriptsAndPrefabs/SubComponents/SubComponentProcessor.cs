using System;
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
        [Space(15)] public PlayerAnimation_Data animation_Data;
        [Space(15)] public CharacterMovement_Data characterMovement_Data;
        [Space(15)] public Interaction_Data interaction_Data;
        [Space(15)] public AIController_Data aIController_Data;

        public void InitSubCompProc()
        {
            arrSubComponents = new SubComponent[Enum.GetNames(typeof(SubComponentType)).Length];
            arrSubComponents = GetComponentsInChildren<SubComponent>();
            control = GetComponentInParent<CharacterControl>();

            foreach (var component in arrSubComponents)
            {
                component.OnAwake();
            }
        }
        public void OnEnableSubcomponents()
        {
            foreach (var component in arrSubComponents)
            {
                component.OnComponentEnabled();
            }
        }

        public void FixedUpdateSubComponents()
        {
            foreach ( var component in arrSubComponents)
            {
                component.OnFixedUpdate();
            }
        }
        public void UpdateSubComponents()
        {
            foreach( var component in arrSubComponents)
            {
                component.OnUpdate();
            }
        }
    }
}

