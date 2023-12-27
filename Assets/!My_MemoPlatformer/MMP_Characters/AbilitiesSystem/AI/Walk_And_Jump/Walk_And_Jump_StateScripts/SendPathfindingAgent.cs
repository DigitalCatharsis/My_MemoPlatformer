using UnityEngine;
using UnityEngine.AI;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/SendPathfindinfAgent")]
    public class SendPathfindinfAgent : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (characterState.characterControl.aiProgress.pathfindfingAgent == null)
            {
                var pfAgent = Instantiate(Resources.Load("PathfindingAgent", typeof(GameObject)) as GameObject);
                characterState.characterControl.aiProgress.pathfindfingAgent = pfAgent.GetComponent<PathFindingAgent>();
            }

            characterState.characterControl.aiProgress.pathfindfingAgent.owner = characterState.characterControl;
            characterState.characterControl.aiProgress.pathfindfingAgent.GetComponent<NavMeshAgent>().enabled = false;
            characterState.characterControl.aiProgress.pathfindfingAgent.transform.position = characterState.characterControl.transform.position + (Vector3.up * 0.5f);
            characterState.characterControl.navMeshObstacle.carving = false; //to prevent bug when carving forbids agent to move
            characterState.characterControl.aiProgress.pathfindfingAgent.GoToTarget();
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

            if (characterState.characterControl.aiProgress.pathfindfingAgent.startWalk)
            {
                animator.SetBool(HashManager.Instance.ArrAITransitionParams[(int)AI_Transition.Start_Walking], true);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.ArrAITransitionParams[(int)AI_Transition.Start_Walking], false);
        }
    }

}
