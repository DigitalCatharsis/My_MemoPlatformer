using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/GroundDetector")]
    public class GroundDetector : StateData
    {
        

        [SerializeField][Range(0.01f, 1f)]
        private float _checktime; //Сначала нужно подняться в воздух, а потом проверять isgrounded. Чето типа fail-safe
        [SerializeField] private float distance;
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            

            if (stateInfo.normalizedTime >= _checktime)
            {
                if (IsGrounded(characterState.characterControl))
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
            if (control.contactPoints != null)
            {
                foreach (ContactPoint c in control.contactPoints)
                {
                    float colliderBottom = (control.transform.position.y + control.boxCollider.center.y) - (control.boxCollider.size.y / 2f);
                    float yDiffirence = Mathf.Abs(c.point.y - colliderBottom);

                    if (yDiffirence < 0.01f)
                    {
                        if (Mathf.Abs(control.Rigid_Body.velocity.y) < 0.01f)
                        {
                            return true;
                        }                        
                    }
                }
            }

            if (control.Rigid_Body.velocity.y < 0f)
            {
                foreach (GameObject o in control.bottomSpheres)
                {
                    Debug.DrawRay(o.transform.position, -Vector3.up * 0.7f, Color.yellow);

                    RaycastHit hit;
                    if (Physics.Raycast(o.transform.position, -Vector3.up, out hit, distance))
                    {
                        if (!control.ragdollParts.Contains(hit.collider) 
                            && !Ledge.IsLedge(hit.collider.gameObject) 
                            && !Ledge.IsLedgeChecker(hit.collider.gameObject)
                            && !Ledge.IsCharacter(hit.collider.gameObject))
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
