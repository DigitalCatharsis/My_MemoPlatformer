using UnityEngine.AI;
using UnityEngine;
using System;
using System.Drawing;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/StartWalking")]
    public class StartWalking : StateData
    {
        public Vector3 targetDir = new Vector3();
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            WalkStraightTowardsTarget(characterState.characterControl);
        }

        private void WalkStraightTowardsTarget(CharacterControl control)
        {
            targetDir = control.aiProgress.pathfindfingAgent.startSphere.transform.position
                - control.transform.position;

            if (targetDir.z > 0f)
            {
                control.moveLeft = false;
                control.moveRight = true;
            }
            else
            {
                control.moveLeft = true;
                control.moveRight = false;
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(AI_Walk_Transitions.Jump_Platform.ToString(), false);
            animator.SetBool(AI_Walk_Transitions.Fall_Platform.ToString(), false);
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {            
            //Debug.DrawLine(control.transform.position, control.aiProgress.pathfindfingAgent.startSphere.transform.position, UnityEngine.Color.magenta,0.1f);
            if (characterState.characterControl.aiProgress.pathfindfingAgent.startSphere.transform.position.y < characterState.characterControl.aiProgress.pathfindfingAgent.endSphere.transform.position.y) //Если это подъем
            {
                //Jumping
                if (characterState.characterControl.aiProgress.GetDistanceToDestination() < 0.015f) //how close are we to the checkpoint    //Здесь часто бывает баг (когда иди бегает вокруг Start Point) из-за разных смещений платформы или ИИ относительно друг друга. Увелич да < 0.1f для дебага
                {
                    //Debug.DrawLine(control.aiProgress.pathfindfingAgent.startSphere.transform.position, control.transform.position, UnityEngine.Color.green, 2.5f);
                    characterState.characterControl.moveLeft = false;
                    characterState.characterControl.moveRight = false;
                    //Debug.Log($"<color=red>{control.moveLeft = false} </color>");
                    animator.SetBool(AI_Walk_Transitions.Jump_Platform.ToString(), true);
                }
            }
            //fall
            if (characterState.characterControl.aiProgress.pathfindfingAgent.startSphere.transform.position.y > characterState.characterControl.aiProgress.pathfindfingAgent.endSphere.transform.position.y)
            {
                animator.SetBool(AI_Walk_Transitions.Fall_Platform.ToString(), true);
            }


        }
    }
}
