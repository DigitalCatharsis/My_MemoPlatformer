using UnityEngine.AI;
using UnityEngine;
using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Diagnostics;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/AiTransitionCondition")]
    public class AiTransitionCondition : StateData
    {
        public enum AiTransitionType
        {
            RUN_TO_WALK,

        }

        public AiTransitionType aiTransition;
        public AI_TYPE nextAI;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);
            if (TransitionToNextAI(control))
            {
                control.aiController.TriggerAI(nextAI);
            }
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        bool TransitionToNextAI(CharacterControl control)
        {
            if (aiTransition == AiTransitionType.RUN_TO_WALK)
            {
                Vector3 dist = control.aiProgress.pathfindfingAgent.startSphere.transform.position - control.transform.position; //distance between checkpoint and character

                if (Vector3.SqrMagnitude(dist) < 2f)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
