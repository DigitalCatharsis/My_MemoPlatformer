using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace My_MemoPlatformer
{
    public enum AI_Walk_Transitions
    {
        Start_Walking,
        Jump_Platform,
        Fall_Platform,
        Start_Running
    }

    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/SendPathfindinfAgent")]
    public class SendPathfindinfAgent : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);

            if (control.aiProgress.pathfindfingAgent == null)
            {
                GameObject pfAgent = Instantiate(Resources.Load("PathfindingAgent", typeof (GameObject)) as GameObject);
                control.aiProgress.pathfindfingAgent = pfAgent.GetComponent<PathFindingAgent>();
            }

            control.aiProgress.pathfindfingAgent.GetComponent<NavMeshAgent>().enabled = false;
            control.aiProgress.pathfindfingAgent.transform.position = control.transform.position;
            control.aiProgress.pathfindfingAgent.GoToTarget();
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);
            if (control.aiProgress.pathfindfingAgent.startWalk)
            {
                animator.SetBool(AI_Walk_Transitions.Start_Walking.ToString(), true);
                animator.SetBool(AI_Walk_Transitions.Start_Running.ToString(), true);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(AI_Walk_Transitions.Start_Walking.ToString(), false);
            animator.SetBool(AI_Walk_Transitions.Start_Running.ToString(), false);
        }
    }

}
