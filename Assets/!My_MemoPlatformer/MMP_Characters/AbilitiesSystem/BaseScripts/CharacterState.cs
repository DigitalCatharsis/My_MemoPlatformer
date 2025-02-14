using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class CharacterState : StateMachineBehaviour
    {
        public CharacterControl characterControl;
        [Space(10)]
        public List<CharacterAbility> ListAbilityData = new List<CharacterAbility>();
        [Space(10)]
        public CharacterAbility[] arrAbilities;

        public BlockingObj_Data BlockingObj_Data => characterControl.subComponentProcessor.blockingObj_Data;
        public Ragdoll_Data RagdollData => characterControl.subComponentProcessor.ragdoll_Data;
        public BoxCollider_Data BoxCollider_Data => characterControl.subComponentProcessor.boxCollider_Data;
        public VerticalVelocity_Data Vertical_Velocity_Data => characterControl.subComponentProcessor.verticalVelocity_Data;
        public MomentumCalculator_Data MomentumCalculator_Data => characterControl.subComponentProcessor.momentumCalculator_Data;
        public Rotation_Data Rotation_Data => characterControl.subComponentProcessor.rotation_Data;
        public Jump_Data Jump_Data => characterControl.subComponentProcessor.jump_Data;
        public CollisionSpheres_Data COLLISION_SPHERE_DATA => characterControl.subComponentProcessor.collisionSpheres_Data;
        public Ground_Data Ground_Data => characterControl.subComponentProcessor.ground_Data;
        public Attack_Data Attack_Data => characterControl.subComponentProcessor.attack_Data;
        public PlayerAnimation_Data Player_Animation_Data => characterControl.subComponentProcessor.animation_Data;
        public AIController_Data AI_CONTROLLER_DATA => characterControl.AICONTROLLER_DATA;
        public CharacterMovement_Data CharacterMovement_Data => characterControl.CHARACTER_MOVEMENT_DATA;
        public AIController_Data AIController_Data => characterControl.AICONTROLLER_DATA;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (int i = 0; i < arrAbilities.Length; i++)
            {
                arrAbilities[i].OnEnter(this, animator, stateInfo);

                if (characterControl.PLAYER_ANIMATION_DATA.currentRunningAbilities_Dictionary.ContainsKey(arrAbilities[i]))
                {
                    characterControl.PLAYER_ANIMATION_DATA.currentRunningAbilities_Dictionary[arrAbilities[i]] += 1;
                }
                else
                {
                    characterControl.PLAYER_ANIMATION_DATA.currentRunningAbilities_Dictionary.Add(arrAbilities[i], 1);
                }
            }
        }

        public void UpdateAll(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            for (int i = 0; i < arrAbilities.Length; i++)
            {
                arrAbilities[i].UpdateAbility(characterState, animator, stateInfo);
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)    //this is a place where was fixed move forward x4 speed bug with uncessesary foreach
        {
            UpdateAll(this, animator, stateInfo);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            for (int i = 0; i < arrAbilities.Length; i++)
            {
                try
                {
                    arrAbilities[i].OnExit(this, animator, stateInfo);

                    if (characterControl.PLAYER_ANIMATION_DATA.currentRunningAbilities_Dictionary.ContainsKey(arrAbilities[i]))
                    {
                        characterControl.PLAYER_ANIMATION_DATA.currentRunningAbilities_Dictionary[arrAbilities[i]] -= 1;

                        if (characterControl.PLAYER_ANIMATION_DATA.currentRunningAbilities_Dictionary[arrAbilities[i]] <= 0)
                        {
                            characterControl.PLAYER_ANIMATION_DATA.currentRunningAbilities_Dictionary.Remove(arrAbilities[i]);
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.Log(e);
                    throw e;
                }
            }
        }
    }
}