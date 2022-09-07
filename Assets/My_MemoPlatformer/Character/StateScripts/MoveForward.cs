using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    [CreateAssetMenu(fileName = "New state", menuName = " My_MemoPlatformer/AbilityData/MoveForward")]
    public class MoveForward : StateData
    {
        [SerializeField] private bool _constant; //move no matter what
        [SerializeField] private AnimationCurve _speedGraph;
        [SerializeField] private float _speed;
        [SerializeField] private float _blockDistance;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);

            if (control.Jump)
            {
                animator.SetBool(TransitionParameter.Jump.ToString(), true);
            }

            if (_constant)
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
                control.MoveForward(_speed, _speedGraph.Evaluate(stateInfo.normalizedTime));
            }
        }

        private void ControlledMove(CharacterControl control, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (control.MoveRight && control.MoveLeft)
            {
                animator.SetBool(TransitionParameter.Move.ToString(), false);
                return;
            }

            if (!control.MoveRight && !control.MoveLeft)
            {
                animator.SetBool(TransitionParameter.Move.ToString(), false);
                return;
            }

            if (control.MoveRight)
            {

                control.transform.rotation = Quaternion.Euler(0f, 0, 0f);
                if (!CheckFront(control))
                {
                    control.MoveForward(_speed, _speedGraph.Evaluate(stateInfo.normalizedTime));
                }
            }

            if (control.MoveLeft)
            {
                {

                    control.transform.rotation = Quaternion.Euler(0f, 180, 0f);
                    if (!CheckFront(control))
                    {
                        control.MoveForward(_speed, _speedGraph.Evaluate(stateInfo.normalizedTime));
                    }
                }
            }
        }



        bool CheckFront(CharacterControl control)
        {
            foreach (GameObject o in control.frontSpheres)
            {
                Debug.DrawRay(o.transform.position, control.transform.forward * 0.3f, Color.yellow);
                RaycastHit hit;
                if (Physics.Raycast(o.transform.position, control.transform.forward, out hit, _blockDistance))
                {
                    if (!control.RagdollParts.Contains(hit.collider))  //Проверка, что задетый коллайдер не часть колайдеров radoll
                    {
                        if (!IsBodyPart(hit.collider))  // Проверка, что мы не задеваем...коллайдер меша. В инспекторе - это прямоугольный коллайдер
                        {
                            return true;
                        }
                    }
                }
            }

            return false;

        }

        private bool IsBodyPart (Collider col)
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

            if (control.RagdollParts.Contains(col)) //thats a part of ragdoll
            {
                return true;
            }

            return false;

        }
    }


}