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
            

            Vector3 dir = characterState.characterControl.aiProgress.pathfindfingAgent.startSphere.transform.position - characterState.characterControl.transform.position;

            if (dir.z > 0f)
            {
                characterState.characterControl.moveLeft = false;
                characterState.characterControl.moveRight = true;
            }
            else
            {
                characterState.characterControl.moveLeft = true;
                characterState.characterControl.moveRight = false;
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(AI_Walk_Transitions.Jump_Platform.ToString(), false);
            animator.SetBool(AI_Walk_Transitions.Fall_Platform.ToString(), false);
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
            Vector3 dist = characterState.characterControl.aiProgress.pathfindfingAgent.startSphere.transform.position - characterState.characterControl.transform.position; //distance between checkpoint and character

            //Debug.DrawLine(control.transform.position, control.aiProgress.pathfindfingAgent.startSphere.transform.position, UnityEngine.Color.magenta,0.1f);
            if (characterState.characterControl.aiProgress.pathfindfingAgent.startSphere.transform.position.y < characterState.characterControl.aiProgress.pathfindfingAgent.endSphere.transform.position.y) //Если это подъем
            {
                //Jumping
                if (Vector3.SqrMagnitude(dist) < 0.01f) //how close are we to the checkpoint    //Здесь часто бывает баг (когда иди бегает вокруг Start Point) из-за разных смещений платформы или ИИ относительно друг друга. Увелич да < 0.1f для дебага
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

            //straight
            if (characterState.characterControl.aiProgress.pathfindfingAgent.startSphere.transform.position.y == characterState.characterControl.aiProgress.pathfindfingAgent.endSphere.transform.position.y)   //Цель перед нами
            {
                if (Vector3.SqrMagnitude(dist) < 0.5f) //чтобы не топтался в персонажа
                {
                    characterState.characterControl.moveLeft = false;
                    characterState.characterControl.moveRight = false;

                    Vector3 playerDist = characterState.characterControl.transform.position - CharacterManager.Instance.GetPlayableCharacter().transform.position;
                    if (playerDist.sqrMagnitude > 1f) //if player moved somewhere else
                    {
                        animator.gameObject.SetActive(false);
                        animator.gameObject.SetActive(true); //Ищи по новой
                    }

                    //temp attack
                    //else
                    //{
                        //if (CharacterManager.Instance.GetPlayableCharacter().damageDetector.damageTaken == 0) //want to attack player if its alive (didnt date any damage)
                        //{
                        //    if (control.IsFacingForward())
                        //    {
                        //        control.moveRight = true;
                        //        control.moveLeft = false;
                        //        control.attack = true;
                        //    }
                        //    else
                        //    {
                        //        control.moveRight = false;
                        //        control.moveLeft = true;
                        //        control.attack = true;
                        //    }
                        //}
                    //}
                }
            }
        }
    }
}
