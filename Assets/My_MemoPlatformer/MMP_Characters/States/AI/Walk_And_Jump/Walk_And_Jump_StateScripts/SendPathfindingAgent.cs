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


            if (characterState.characterControl.aiProgress.pathfindfingAgent == null)
            {
                GameObject pfAgent = Instantiate(Resources.Load("PathfindingAgent", typeof(GameObject)) as GameObject);
                characterState.characterControl.aiProgress.pathfindfingAgent = pfAgent.GetComponent<PathFindingAgent>();
            }

            characterState.characterControl.aiProgress.pathfindfingAgent.owner = characterState.characterControl;
            characterState.characterControl.aiProgress.pathfindfingAgent.GetComponent<NavMeshAgent>().enabled = false;
            characterState.characterControl.aiProgress.pathfindfingAgent.transform.position = characterState.characterControl.transform.position;
            characterState.characterControl.navMeshObstacle.carving = false; //to prevent bug when carving forbids agent to move
            characterState.characterControl.aiProgress.pathfindfingAgent.GoToTarget();
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

            if (characterState.characterControl.aiProgress.pathfindfingAgent.startWalk)
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
