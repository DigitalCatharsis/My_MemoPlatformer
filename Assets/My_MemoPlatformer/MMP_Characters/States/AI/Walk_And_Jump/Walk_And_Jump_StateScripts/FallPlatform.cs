using UnityEngine.AI;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/FallPlatform")]
    public class FallPlatform : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);

            if (control.transform.position.z < control.aiProgress.pathfindfingAgent.transform.position.z)
            {
                control.FaceForward(true);
            }
            else if (control.transform.position.z > control.aiProgress.pathfindfingAgent.transform.position.z)
            {
                control.FaceForward(false);
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);
            if (control.IsFacingForward())
            {
                if (control.transform.position.z < control.aiProgress.pathfindfingAgent.endSphere.transform.position.z)
                {
                    control.moveRight = true;
                    control.moveLeft = false;
                }
                else
                {
                    control.moveRight = false;
                    control.moveLeft = false;

                    animator.gameObject.SetActive(false);    //repeat seatch process
                    animator.gameObject.SetActive(true);
                }
            }
            else
            {
                if (control.transform.position.z > control.aiProgress.pathfindfingAgent.endSphere.transform.position.z)
                {
                    control.moveLeft = true;
                    control.moveRight = false;
                }
                else
                {
                    control.moveRight = false;
                    control.moveLeft = false;

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