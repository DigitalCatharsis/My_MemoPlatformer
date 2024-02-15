using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New State", menuName = "My_MemoPlatformer/AbilityData/InstantTransition")]
    public class InstantTransition : CharacterAbility
    {
        public Instant_Transition_States transitionTo;
        public List<TransitionConditionType> transitionConditions = new List<TransitionConditionType>();
        public float crossFade;
        public float offset;
        public bool ignoreAttackAbility;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!Interfered(characterState.characterControl))
            {
                if (TransitionConditionChecker.MakeTransition(characterState.characterControl, transitionConditions))
                {
                    characterState.Player_Animation_Data.instantTransitionMade = true;
                    MakeInstantTransition(characterState.characterControl);
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.Player_Animation_Data.instantTransitionMade = false;
        }

        void MakeInstantTransition(CharacterControl control)
        {
            if (crossFade <= 0f)
            {
                control.skinnedMeshAnimator.Play(
                    HashManager.Instance.arrInstantTransitionStates[(int)transitionTo], 0);
            }
            else
            {
                if (DebugContainer_Data.Instance.debug_TransitionTiming)
                {
                    Debug.Log("Instant transition to: " + transitionTo.ToString() + " - CrossFade: " + crossFade);
                }

                if (offset <= 0f)
                {
                    control.skinnedMeshAnimator.CrossFade(HashManager.Instance.arrInstantTransitionStates[(int)transitionTo],crossFade, 0);
                }
                else
                {
                    control.skinnedMeshAnimator.CrossFade(HashManager.Instance.arrInstantTransitionStates[(int)transitionTo], crossFade, 0, offset);
                }
            }
        }

        private bool Interfered(CharacterControl control)
        {
            if (control.PLAYER_ANIMATION_DATA.lockTransition)
            {
                return true;
            }

            if (control.PLAYER_ANIMATION_DATA.instantTransitionMade)
            {
                return true;
            }

            if (control.skinnedMeshAnimator.GetInteger(
                HashManager.Instance.arrMainParams[(int)MainParameterType.TransitionIndex]) != 0)
            {
                return true;
            }

            if (!ignoreAttackAbility)
            {
                if (control.PLAYER_ANIMATION_DATA.IsRunning(typeof(Attack)))
                {
                    return true;
                }
            }

            AnimatorStateInfo nextInfo = control.skinnedMeshAnimator.GetNextAnimatorStateInfo(0);

            if (nextInfo.shortNameHash == HashManager.Instance.arrInstantTransitionStates[(int)transitionTo])
            {
                return true;
            }

            return false;
        }
    }
}