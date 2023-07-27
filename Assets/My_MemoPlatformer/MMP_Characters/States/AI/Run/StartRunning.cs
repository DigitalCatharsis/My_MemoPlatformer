using UnityEngine.AI;
using UnityEngine;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/StartRunning")]
    public class StartRunning : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            Vector3 dir = characterState.characterControl.aiProgress.pathfindfingAgent.startSphere.transform.position - characterState.characterControl.transform.position;

            if (dir.z > 0f)
            {
                characterState.characterControl.FaceForward(true);
                characterState.characterControl.moveLeft = false;
                characterState.characterControl.moveRight = true;
            }
            else
            {
                characterState.characterControl.FaceForward(false);
                characterState.characterControl.moveLeft = true; 
                characterState.characterControl.moveRight = false;
            }
            
            if (characterState.characterControl.aiProgress.GetDistanceToDestination() > 2f)
            {
                characterState.characterControl.turbo = true;
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (characterState.characterControl.aiProgress.GetDistanceToDestination() < 2f) //чтобы не топтался в персонажа
            {
                characterState.characterControl.moveLeft = false;
                characterState.characterControl.moveRight = false;
                characterState.characterControl.turbo = false;
            }
        }
    }
}
