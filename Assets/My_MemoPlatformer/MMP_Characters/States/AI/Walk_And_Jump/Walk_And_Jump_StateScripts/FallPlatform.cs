using UnityEngine.AI;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/FallPlatform")]
    public class FallPlatform : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            

            if (characterState.characterControl.transform.position.z < characterState.characterControl.aiProgress.pathfindfingAgent.transform.position.z)
            {
                characterState.characterControl.FaceForward(true);
            }
            else if (characterState.characterControl.transform.position.z > characterState.characterControl.aiProgress.pathfindfingAgent.transform.position.z)
            {
                characterState.characterControl.FaceForward(false);
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
            if (characterState.characterControl.IsFacingForward())
            {
                if (characterState.characterControl.transform.position.z < characterState.characterControl.aiProgress.pathfindfingAgent.endSphere.transform.position.z)
                {
                    characterState.characterControl.moveRight = true;
                    characterState.characterControl.moveLeft = false;
                }
                else
                {
                    characterState.characterControl.moveRight = false;
                    characterState.characterControl.moveLeft = false;

                    animator.gameObject.SetActive(false);    //repeat seatch process
                    animator.gameObject.SetActive(true);
                }
            }
            else
            {
                if (characterState.characterControl.transform.position.z > characterState.characterControl.aiProgress.pathfindfingAgent.endSphere.transform.position.z)
                {
                    characterState.characterControl.moveLeft = true;
                    characterState.characterControl.moveRight = false;
                }
                else
                {
                    characterState.characterControl.moveRight = false;
                    characterState.characterControl.moveLeft = false;

                    animator.gameObject.SetActive(false);    //repeat seatch process
                    animator.gameObject.SetActive(true);
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }
    }

}