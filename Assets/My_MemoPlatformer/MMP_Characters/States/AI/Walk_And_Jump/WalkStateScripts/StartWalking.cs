using UnityEngine.AI;
using UnityEngine;
using System;
using System.Drawing;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/StartWalking")]
    public class StartWalking : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);

            Vector3 dir = control.aiProgress.pathfindfingAgent.startSphere.transform.position - control.transform.position;

            if (dir.z >0f)
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

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);
            Vector3 dist = control.aiProgress.pathfindfingAgent.startSphere.transform.position - control.transform.position;

            Debug.Log(Vector3.SqrMagnitude(dist));

            if (Vector3.SqrMagnitude(dist) < 0.01f) //how close are we to the checkpoint
            {
                Debug.DrawLine(control.aiProgress.pathfindfingAgent.startSphere.transform.position, control.transform.position, UnityEngine.Color.magenta, 2.5f);
                control.moveLeft=false;
                control.moveRight = false;

                Debug.Log($"<color=red>{control.moveLeft = false} </color>");

                if (control.aiProgress.pathfindfingAgent.startSphere.transform.position.y < control.aiProgress.pathfindfingAgent.endSphere.transform.position.y)
                {
                    animator.SetBool(AI_Walk_Transitions.Jump_Platform.ToString(), true);
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(AI_Walk_Transitions.Jump_Platform.ToString(), false);
        }
    }

}