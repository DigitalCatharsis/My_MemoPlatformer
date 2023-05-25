using UnityEngine.AI;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/JumpPlatform")]
    public class JumpPlatform : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);

            control.jump = true;
            control.moveUp = true;

            if (control.aiProgress.pathfindfingAgent.startSphere.transform.position.z < control.aiProgress.pathfindfingAgent.endSphere.transform.position.z)
            {
                control.FaceForward(true);
            }
            else
            {
                control.FaceForward(false);
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);

            //Diffirence betwen character's top sphere (coliistion emulation) and End sphere of the pathfinding agent
            float topDist = control.aiProgress.pathfindfingAgent.endSphere.transform.position.y - control.frontSpheres[1].transform.position.y;
            float bottomDist = control.aiProgress.pathfindfingAgent.endSphere.transform.position.y - control.frontSpheres[0].transform.position.y;

            //Debug.DrawLine(control.aiProgress.pathfindfingAgent.endSphere.transform.position, control.frontSpheres[1].transform.position, Color.magenta, 2.5f);
            //Debug.DrawLine(control.aiProgress.pathfindfingAgent.endSphere.transform.position, control.frontSpheres[0].transform.position, Color.red, 2.5f);

            if (topDist < 1.5f && bottomDist > 0.5f)  //bottomDist > 0.5f means it is on the same platform
            {
                if (control.IsFacingForward())
                {
                    control.moveRight = true;
                    control.moveLeft = false;
                }
                else
                {
                    control.moveRight = false;
                    control.moveLeft = true;
                }
            }

            if (bottomDist < 0.5f)
            {
                control.moveRight = false;
                control.moveLeft = false;
                control.moveUp=false;
                control.jump=false;

                animator.gameObject.SetActive(false);
                animator.gameObject.SetActive(true);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }
    }

}