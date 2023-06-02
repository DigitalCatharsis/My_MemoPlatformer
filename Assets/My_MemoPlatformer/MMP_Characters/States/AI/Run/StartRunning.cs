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
            CharacterControl control = characterState.GetCharacterControl(animator);

            Vector3 dir = control.aiProgress.pathfindfingAgent.startSphere.transform.position - control.transform.position;

            if (dir.z > 0f)
            {
                control.FaceForward(true);
                control.moveLeft = false;
                control.moveRight = true;
            }
            else
            {
                control.FaceForward(false);
                control.moveLeft = true;
                control.moveRight = false;
            }

            control.turbo = true;
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);
            Vector3 dist = control.aiProgress.pathfindfingAgent.startSphere.transform.position - control.transform.position;

            if (Vector3.SqrMagnitude(dist) < 2f) //чтобы не топтался в персонажа
            {
                control.moveLeft = false;
                control.moveRight = false;
                control.turbo = false;
            }
        }
    }
}
