using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    [CreateAssetMenu(fileName = "New state", menuName = " My_MemoPlatformer/AbilityData/MoveForward")]
    public class MoveForward : StateData
    {
        [SerializeField] private AnimationCurve SpeedGraph;
        [SerializeField] private float Speed;
        [SerializeField] private float BlockDistance;

        private bool _self;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);

            if(control.Jump)
            {
                animator.SetBool(TransitionParameter.Jump.ToString(), true);
            }

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
                    control.transform.Translate(Vector3.forward * Speed * SpeedGraph.Evaluate(stateInfo.normalizedTime) * Time.deltaTime);
                }

            }

            if (control.MoveLeft)
            {
                {

                    control.transform.rotation = Quaternion.Euler(0f, 180, 0f);
                    if (!CheckFront(control))
                    {
                        control.transform.Translate(Vector3.forward * Speed * SpeedGraph.Evaluate(stateInfo.normalizedTime) * Time.deltaTime);
                    }
                }
            }
        }

        bool CheckFront(CharacterControl control)
        {
            foreach (GameObject o in control.frontSpheres)
            {
                _self = false;
                Debug.DrawRay(o.transform.position, control.transform.forward * 0.3f, Color.yellow);
                RaycastHit hit;
                if (Physics.Raycast(o.transform.position, control.transform.forward, out hit, BlockDistance))
                {
                    foreach(Collider c in control.RagdollParts)
                    {
                        if(c.gameObject == hit.collider.gameObject) //if raycast hitting collider from ragdoll ignore it
                        {
                            _self = true;
                            break;
                        }
                    }

                    if (!_self)
                    {
                        return true;
                    } 
                    
                }
            }
            return false;
        }
    }
}