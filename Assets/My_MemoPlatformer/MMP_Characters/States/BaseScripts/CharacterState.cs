using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    public class CharacterState : StateMachineBehaviour
    {
        public CharacterControl characterControl;

        public List<StateData> ListAbilityData = new List<StateData>();

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
                if (!characterControl.animationProgress.currentRunningAbilities.Contains(d))
                {
                    characterControl.animationProgress.currentRunningAbilities.Add(d);
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

                if (characterControl.animationProgress.currentRunningAbilities.Contains(d))
                {
                    characterControl.animationProgress.currentRunningAbilities.Remove(d);
                }
            }
        }
    }
}