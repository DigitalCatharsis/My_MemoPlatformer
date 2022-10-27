using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = " My_MemoPlatformer/AbilityData/GroundDetector")]
    public class GroundDetector : StateData
    {
        [SerializeField] private float distance;

        [Range(0.01f, 1f)][SerializeField] private float Checktime; //Сначала нужно подняться в воздух, а потом проверять isgrounded. Чето типа fail-safe
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);

            if (stateInfo.normalizedTime >= Checktime)
            {
                if (IsGrounded(control))
                {
                    animator.SetBool(TransitionParameter.Grounded.ToString(), true);
                }
                else
                {
                    animator.SetBool(TransitionParameter.Grounded.ToString(), false);
                }
            }

        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        bool IsGrounded(CharacterControl control)
        {
            if (control.Rigid_Body.velocity.y > -0.001f && control.Rigid_Body.velocity.y <= 0.0f)
            {
                return true;
            }

            if(control.Rigid_Body.velocity.y < 0f)
            {
                foreach (GameObject o in control.bottomSpheres)
                {
                    Debug.DrawRay(o.transform.position, -Vector3.up * 0.7f, Color.yellow);

                    RaycastHit hit;
                    if (Physics.Raycast(o.transform.position, -Vector3.up, out hit, distance))
                    {
                        if (!control.ragdollParts.Contains(hit.collider))
                        {
                            return true;
                        }
                        
                    }
                }
            }
            return false;
        }
    }

}
