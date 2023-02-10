using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/MoveForward")]
    public class MoveForward : StateData
    {
        public bool allowEarlyTurn; //Prevent turning when running from idle
        public bool lockDirection;
        public bool constant; //move no matter what
        public AnimationCurve speedGraph;
        public float speed;
        public float blockDistance;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (allowEarlyTurn)
            {
                CharacterControl control = characterState.GetCharacterControl(animator);

                if (control.moveLeft)
                {
                    control.FaceForward(false);
                }
                if (control.moveRight)
                {
                    control.FaceForward(true);
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);

            if (control.jump)
            {
                animator.SetBool(TransitionParameter.Jump.ToString(), true);
            }

            if (constant)
            {
                ConstantMove(control, animator, stateInfo);
            }
            else
            {
                ControlledMove(control, animator, stateInfo);
            }
        }

        private void ConstantMove(CharacterControl control, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!CheckFront(control))
            {
                control.MoveForward(speed, speedGraph.Evaluate(stateInfo.normalizedTime));
            }
        }

        private void ControlledMove(CharacterControl control, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (control.moveRight && control.moveLeft)
            {
                animator.SetBool(TransitionParameter.Move.ToString(), false);
                return;
            }

            if (!control.moveRight && !control.moveLeft)
            {
                animator.SetBool(TransitionParameter.Move.ToString(), false);
                return;
            }

            if (control.moveRight)
            {
                if (!CheckFront(control))
                {
                    control.MoveForward(speed, speedGraph.Evaluate(stateInfo.normalizedTime));
                }
            }

            if (control.moveLeft)
            {
                {
                    if (!CheckFront(control))
                    {
                        control.MoveForward(speed, speedGraph.Evaluate(stateInfo.normalizedTime));
                    }
                }
            }
            CheckTurn(control);
        }

        private void CheckTurn(CharacterControl control)
        {
            if (!lockDirection)
            {
                if (control.moveRight)
                {
                    control.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
                if (control.moveLeft)
                {
                    control.transform.rotation = Quaternion.Euler(0f, 180, 0f);
                }
            }
        }

        bool CheckFront(CharacterControl control)  //Проверка на коллизии
        {
            foreach (GameObject o in control.frontSpheres)
            {
                Debug.DrawRay(o.transform.position, control.transform.forward * 0.3f, Color.yellow);
                RaycastHit hit;
                if (Physics.Raycast(o.transform.position, control.transform.forward, out hit, blockDistance))
                {
                    if (!control.ragdollParts.Contains(hit.collider))  //Проверка, что задетый коллайдер не часть колайдеров radoll
                    {
                        if (!IsBodyPart(hit.collider) && !Ledge.IsLedge(hit.collider.gameObject) && !Ledge.IsLedgeChecker(hit.collider.gameObject))  // Проверка, что мы ничего не задеваем, включая Ledge (платформы, за котоыре можно зацепиться)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;

        }

        private bool IsBodyPart(Collider col)
        {
            CharacterControl control = col.transform.root.GetComponent<CharacterControl>();

            if (control == null)
            {
                return false;
            }

            if (control.gameObject == col.gameObject)  //thats a root of the character
            {
                return false;
            }

            if (control.ragdollParts.Contains(col)) //thats a part of ragdoll
            {
                return true;
            }

            return false;

        }
    }


}