using UnityEditor;
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
            var platformDistance = characterState.characterControl.aiProgress.pathfindingAgent.endSphere.transform.position.y
                - characterState.COLLISION_SPHERE_DATA.frontSpheres[0].transform.position.y;

            if (platformDistance > 0.5f) 
            {
                if (DebugContainer_Data.Instance.debug_AI)
                {
                    EditorApplication.isPaused = true;
                }

                //TODO Добавить проверку относительно стартовой сферы??

                if (characterState.characterControl.aiProgress.pathfindingAgent.startSphere.transform.position.z <
                    characterState.characterControl.aiProgress.pathfindingAgent.endSphere.transform.position.z)
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

            if (platformDistance < 0.5f)  //means it is on the same platform
            {
                if (DebugContainer_Data.Instance.debug_AI)
                {
                    EditorApplication.isPaused = true;
                }

                characterState.characterControl.moveRight = false;
                characterState.characterControl.moveLeft = false;
                characterState.characterControl.moveUp = false;
                characterState.characterControl.jump = false;
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }
    }
}