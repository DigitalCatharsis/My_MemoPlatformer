using UnityEngine;
using UnityEngine.AI;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/SendPathfindinfAgent")]
    public class SendPathfindinfAgent : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.AICONTROLLER_DATA.aiStatus = Ai_Status.Sending_Pathfinding_Agent.ToString();
            characterState.characterControl.AICONTROLLER_DATA.pathfindingAgent.owner = characterState.characterControl;
            characterState.characterControl.AICONTROLLER_DATA.pathfindingAgent.GetComponent<NavMeshAgent>().enabled = false;
            characterState.characterControl.AICONTROLLER_DATA.pathfindingAgent.transform.position = characterState.characterControl.transform.position + (Vector3.up * 0.5f);
            characterState.characterControl.navMeshObstacle.carving = false; //to prevent bug when carving forbids agent to move
            characterState.characterControl.AICONTROLLER_DATA.pathfindingAgent.ReinitAgent_And_CheckDestination();
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (characterState.characterControl.AICONTROLLER_DATA.pathfindingAgent.hasFinishedPathfind)
            {
                animator.SetBool(HashManager.Instance.arrAITransitionParams[(int)AI_Transition.Start_Walking], true);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.arrAITransitionParams[(int)AI_Transition.Start_Walking], false);
        }
    }
}
