using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace My_MemoPlatformer
{

    public class CharacterState : StateMachineBehaviour
    {
        public CharacterControl characterControl;
        [Space (10)]
        public List<StateData> ListAbilityData = new List<StateData>();
        public BlockingObj_Data BlockingObjData => characterControl.subComponentProcessor.blockingObjData;
        public Ragdoll_Data RagdollData => characterControl.subComponentProcessor.ragdollData;
        public BoxCollider_Data BoxCollider_Data => characterControl.subComponentProcessor.boxCollider_Data;
        public VerticalVelocity_Data VerticalVelocity_Data => characterControl.subComponentProcessor.verticalVelocity_Data;
        public MomentumCalculator_Data MomentumCalculator_Data => characterControl.subComponentProcessor.momentumCalculator_Data;
        public PlayerRotation_Data PlayerRotation_Data => characterControl.subComponentProcessor.playerRotation_Data;
        public PlayerJump_Data PlayerJump_Data => characterControl.subComponentProcessor.playerJump_Data;
        public CollisionSpheres_Data CollisionSpheres_Data => characterControl.subComponentProcessor.collisionSpheres_Data;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (characterControl == null)  //prevent bug in animator editor, when moving a state
            {
                characterControl = animator.transform.root.gameObject.GetComponent<CharacterControl>();
                characterControl.CacheCharacterControl(animator);
            }

            foreach (StateData d in ListAbilityData)
            {
                d.OnEnter(this, animator, stateInfo);
                if (characterControl.animationProgress.currentRunningAbilities.ContainsKey(d))
                {
                    characterControl.animationProgress.currentRunningAbilities[d] += 1;
                }
                else
                {
                    characterControl.animationProgress.currentRunningAbilities.Add(d,1);
                }
            }
        }
        public void UpdateAll(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            foreach (StateData d in ListAbilityData)
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
            foreach (StateData d in ListAbilityData)
            {
                d.OnExit(this, animator, stateInfo);

                if (characterControl.animationProgress.currentRunningAbilities.ContainsKey(d))
                {
                    characterControl.animationProgress.currentRunningAbilities[d] -= 1;

                    if (characterControl.animationProgress.currentRunningAbilities[d] <= 0)
                    {
                        characterControl.animationProgress.currentRunningAbilities.Remove(d);
                    }
                }
            }
        }
    }
}