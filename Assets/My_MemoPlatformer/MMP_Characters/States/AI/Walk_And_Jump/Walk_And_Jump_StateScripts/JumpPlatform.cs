using UnityEngine.AI;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/JumpPlatform")]
    public class JumpPlatform : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {         
            characterState.characterControl.jump = true;
            characterState.characterControl.moveUp = true;

            if (characterState.characterControl.aiProgress.pathfindfingAgent.startSphere.transform.position.z < characterState.characterControl.aiProgress.pathfindfingAgent.endSphere.transform.position.z)
            {
                characterState.characterControl.FaceForward(true);
            }
            else
            {
                characterState.characterControl.FaceForward(false);
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {        
            //Diffirence betwen character's top sphere (coliistion emulation) and End sphere of the pathfinding agent
            float topDist = characterState.characterControl.aiProgress.pathfindfingAgent.endSphere.transform.position.y - characterState.characterControl.collisionSpheres.frontSpheres[1].transform.position.y;
            float bottomDist = characterState.characterControl.aiProgress.pathfindfingAgent.endSphere.transform.position.y - characterState.characterControl.collisionSpheres.frontSpheres[0].transform.position.y;

            //Debug.DrawLine(control.aiProgress.pathfindfingAgent.endSphere.transform.position, control.frontSpheres[1].transform.position, Color.magenta, 2.5f);
            //Debug.DrawLine(control.aiProgress.pathfindfingAgent.endSphere.transform.position, control.frontSpheres[0].transform.position, Color.red, 2.5f);

            if (topDist < 1.5f && bottomDist > 0.5f)  //bottomDist > 0.5f means it is on the same platform
            {
                if (characterState.characterControl.IsFacingForward())
                {
                    characterState.characterControl.moveRight = true;
                    characterState.characterControl.moveLeft = false;
                }
                else
                {
                    characterState.characterControl.moveRight = false;
                    characterState.characterControl.moveLeft = true;
                }   
            }

            if (bottomDist < 0.5f)
            {
                characterState.characterControl.moveRight = false;
                characterState.characterControl.moveLeft = false;
                characterState.characterControl.moveUp=false;
                characterState.characterControl.jump=false;

                animator.gameObject.SetActive(false);
                animator.gameObject.SetActive(true);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }
    }

}