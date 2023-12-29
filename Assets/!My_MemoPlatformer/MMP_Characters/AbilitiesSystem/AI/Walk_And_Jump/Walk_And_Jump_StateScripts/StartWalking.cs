using UnityEngine.AI;
using UnityEngine;
using System;
using System.Drawing;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/StartWalking")]
    public class StartWalking : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

            characterState.characterControl.aiProgress.SetRandomFlyingKick();
            characterState.characterControl.aiController.WalkStraightToTheStartSphere();
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.arrAITransitionParams[(int)AI_Transition.Jump_Platform], false);
            animator.SetBool(HashManager.Instance.arrAITransitionParams[(int)AI_Transition.Fall_Platform], false);
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (characterState.characterControl.attack) 
            {
                return;
            }
            //Debug.DrawLine(control.transform.position, control.aiProgress.pathfindfingAgent.startSphere.transform.position, UnityEngine.Color.magenta,0.1f);
            if (characterState.characterControl.aiProgress.EndSphereIsHigher())
            {
                //Jumping
                if (characterState.characterControl.aiProgress.AIDistanceToStartSphere() < 0.08f) //how close are we to the checkpoint    //Здесь часто бывает баг (когда иди бегает вокруг Start Point) из-за разных смещений платформы или ИИ относительно друг друга. Увелич да < 0.1f для дебага
                {
                    //Debug.DrawLine(control.aiProgress.pathfindfingAgent.startSphere.transform.position, control.transform.position, UnityEngine.Color.green, 2.5f);
                    characterState.characterControl.moveLeft = false;
                    characterState.characterControl.moveRight = false;
                    //Debug.Log($"<color=red>{control.moveLeft = false} </color>");
                    animator.SetBool(HashManager.Instance.arrAITransitionParams[(int)AI_Transition.Jump_Platform], true);
                    return;
                }
            }
            //fall
            if (characterState.characterControl.aiProgress.EndSphereIsLower())
            {
                characterState.characterControl.aiController.WalkStraightToTheEndSphere();

                animator.SetBool(HashManager.Instance.arrAITransitionParams[(int)AI_Transition.Fall_Platform], true);
                return;
            }

            //straight
            if (characterState.characterControl.aiProgress.AIDistanceToStartSphere() > 1.5f)
            {
                characterState.characterControl.turbo = true;
            }
            else
            {
                characterState.characterControl.turbo = false;
            }

            characterState.characterControl.aiController.WalkStraightToTheStartSphere();

            if (characterState.characterControl.aiProgress.AIDistanceToEndSphere() < 1f)
            {
                characterState.characterControl.turbo = false;
                characterState.characterControl.moveLeft = false;
                characterState.characterControl.moveRight = false;
            }

            if (characterState.characterControl.aiProgress.TargetIsOnTheSamePlatform())
            {
                characterState.characterControl.aiProgress.RepositionDestination();
            }
        }
    }
}
