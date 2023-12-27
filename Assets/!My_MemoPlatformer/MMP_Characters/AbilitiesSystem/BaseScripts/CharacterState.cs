using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class CharacterState : StateMachineBehaviour
    {
        public CharacterControl characterControl;
        [Space (10)]
        public List<CharacterAbility> ListAbilityData = new List<CharacterAbility>();
        public BlockingObj_Data BlockingObjData => characterControl.subComponentProcessor.blockingObjData;
        public Ragdoll_Data RagdollData => characterControl.subComponentProcessor.ragdollData;
        public BoxCollider_Data BoxCollider_Data => characterControl.subComponentProcessor.boxCollider_Data;
        public VerticalVelocity_Data VerticalVelocity_Data => characterControl.subComponentProcessor.verticalVelocity_Data;
        public MomentumCalculator_Data MomentumCalculator_Data => characterControl.subComponentProcessor.momentumCalculator_Data;
        public PlayerRotation_Data PlayerRotation_Data => characterControl.subComponentProcessor.playerRotation_Data;
        public PlayerJump_Data PlayerJump_Data => characterControl.subComponentProcessor.playerJump_Data;
        public CollisionSpheres_Data CollisionSpheres_Data => characterControl.subComponentProcessor.collisionSpheres_Data;
        public PlayerGround_Data PlayerGround_Data => characterControl.subComponentProcessor.playerGround_Data;
        public PlayerAttack_Data PlayerAttack_Data => characterControl.subComponentProcessor.playerAttack_Data;
        public PlayerAnimation_Data PlayerAnimation_Data => characterControl.subComponentProcessor.playerAnimation_Data;

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
                if (characterControl.PLAYER_ANIMATION_DATA.currentRunningAbilities.ContainsKey(d))
                {
                    characterControl.PLAYER_ANIMATION_DATA.currentRunningAbilities[d] += 1;
                }
                else
                {
                    characterControl.PLAYER_ANIMATION_DATA.currentRunningAbilities.Add(d,1);
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

                if (characterControl.PLAYER_ANIMATION_DATA.currentRunningAbilities.ContainsKey(d))
                {
                    characterControl.PLAYER_ANIMATION_DATA.currentRunningAbilities[d] -= 1;

                    if (characterControl.PLAYER_ANIMATION_DATA.currentRunningAbilities[d] <= 0)
                    {
                        characterControl.PLAYER_ANIMATION_DATA.currentRunningAbilities.Remove(d);
                    }
                }
            }
        }
    }
}