using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class CharacterState : StateMachineBehaviour
    {
        public CharacterControl characterControl;

        [Space (10)]
        public List<CharacterAbility> ListAbilityData = new List<CharacterAbility>();

        public BlockingObj_Data BlockingObj_Data => characterControl.subComponentProcessor.blockingObj_Data;
        public Ragdoll_Data RagdollData => characterControl.subComponentProcessor.ragdoll_Data;
        public BoxCollider_Data BoxCollider_Data => characterControl.subComponentProcessor.boxCollider_Data;
        public VerticalVelocity_Data Vertical_Velocity_Data => characterControl.subComponentProcessor.verticalVelocity_Data;
        public MomentumCalculator_Data MomentumCalculator_Data => characterControl.subComponentProcessor.momentumCalculator_Data;
        public Rotation_Data Rotation_Data => characterControl.subComponentProcessor.rotation_Data;
        public Jump_Data Jump_Data => characterControl.subComponentProcessor.jump_Data;
        public CollisionSpheres_Data CollisionSpheres_Data => characterControl.subComponentProcessor.collisionSpheres_Data;
        public Ground_Data Ground_Data => characterControl.subComponentProcessor.ground_Data;
        public Attack_Data Attack_Data => characterControl.subComponentProcessor.attack_Data;
        public Animation_Data Animation_Data => characterControl.subComponentProcessor.animation_Data;
        public AIController AI_CONTROLLER => characterControl.aiController;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            //if (characterControl == null)
            //{
            //    characterControl = animator.transform.root.gameObject.GetComponent<CharacterControl>();
            //    characterControl.InitCharactersStates(animator);
            //}

            foreach (CharacterAbility d in ListAbilityData)
            {
                d.OnEnter(this, animator, stateInfo);
                if (characterControl.ANIMATION_DATA.currentRunningAbilities.ContainsKey(d))
                {
                    characterControl.ANIMATION_DATA.currentRunningAbilities[d] += 1;
                }
                else
                {
                    characterControl.ANIMATION_DATA.currentRunningAbilities.Add(d,1);
                }
            }
        }
        public void UpdateAll(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            foreach (CharacterAbility d in ListAbilityData)
            {
                d.UpdateAbility(characterState, animator, stateInfo);
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)    //this is a place where was fixed move forward x4 speed bug with uncessesary foreach
        {
                UpdateAll(this, animator, stateInfo);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (CharacterAbility d in ListAbilityData)
            {
                d.OnExit(this, animator, stateInfo);

                if (characterControl.ANIMATION_DATA.currentRunningAbilities.ContainsKey(d))
                {
                    characterControl.ANIMATION_DATA.currentRunningAbilities[d] -= 1;

                    if (characterControl.ANIMATION_DATA.currentRunningAbilities[d] <= 0)
                    {
                        characterControl.ANIMATION_DATA.currentRunningAbilities.Remove(d);
                    }
                }
            }
        }
    }
}