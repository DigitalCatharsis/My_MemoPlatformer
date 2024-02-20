using System;
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
            var platformDistance = characterState.characterControl.aiProgress.pathfindingAgent.endSphere.transform.position
                - characterState.COLLISION_SPHERE_DATA.frontSpheres[0].transform.position;

            if (platformDistance.y > 0.5f) 
            {
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

            if (Math.Abs(platformDistance.z) < 0.1f)  //means it is on the same platform
            {
                if (platformDistance.y < 0.3f)
                {
                    characterState.characterControl.moveRight = false;
                    characterState.characterControl.moveLeft = false;
                    characterState.characterControl.moveUp = false;
                    characterState.characterControl.jump = false;
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }
    }
}