using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/JumpPlatform")]
    public class JumpPlatform : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {         
            characterState.characterControl.jump = true;
            characterState.characterControl.moveUp = true;
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

            if (characterState.characterControl.attack)
            {
                return;
            }

            //Diffirence betwen character's top sphere (coliistion emulation) and End sphere of the pathfinding agent
            //float topDist = characterState.characterControl.aiProgress.pathfindfingAgent.endSphere.transform.position.y - characterState.characterControl.collisionSpheres.frontSpheres[1].transform.position.y;
            float platformDistance = characterState.characterControl.aiProgress.pathfindingAgent.endSphere.transform.position.y 
                - characterState.CollisionSpheres_Data.frontSpheres[0].transform.position.y;

            //Debug.DrawLine(control.aiProgress.pathfindfingAgent.endSphere.transform.position, control.frontSpheres[1].transform.position, Color.magenta, 2.5f);
            //Debug.DrawLine(control.aiProgress.pathfindfingAgent.endSphere.transform.position, control.frontSpheres[0].transform.position, Color.red, 2.5f);

            if (platformDistance > 0.5f)  //means it is on the same platform
            {
                if (characterState.characterControl.aiProgress.pathfindingAgent.startSphere.transform.position.z < characterState.characterControl.aiProgress.pathfindingAgent.endSphere.transform.position.z)
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

            if (platformDistance < 0.5f)
            {
                characterState.characterControl.moveRight = false;
                characterState.characterControl.moveLeft = false;
                characterState.characterControl.moveUp=false;
                characterState.characterControl.jump=false;

                //characterState.characterControl.aiController.InitializeAI(); //after the climp we reaching new platform without landing mpotion. i need it for now
                //animator.gameObject.SetActive(false);
                //animator.gameObject.SetActive(true);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }
    }

}