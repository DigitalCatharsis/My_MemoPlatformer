using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/GroundDetector")]
    public class GroundDetector : StateData
    {
        [SerializeField] private float distance;

        private GameObject testingSphere;

        public GameObject TestingSphere
        {
            get
            {
                if (testingSphere == null)
                {
                    testingSphere = GameObject.Find("TestingSphere");
                }
                return testingSphere;
            }
        }
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (IsGrounded(characterState.characterControl))
            {
                animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Grounded], true);
            }
            else
            {
                animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Grounded], false);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        bool IsGrounded(CharacterControl control)
        {
            if (control.contactPoints != null)
            {
                foreach (var c in control.contactPoints)
                {
                    var colliderBottom = (control.transform.position.y + control.boxCollider.center.y) - (control.boxCollider.size.y / 2f);
                    var yDiffirence = Mathf.Abs(c.point.y - colliderBottom);

                    if (yDiffirence < 0.01f)
                    {
                        if (Mathf.Abs(control.Rigid_Body.velocity.y) < 0.001f)
                        {
                            //control.animationProgress.ground = c.otherCollider.transform.root.gameObject; //что колайдерит bottom сферы
                            control.animationProgress.ground = c.otherCollider.transform.gameObject; //что колайдерит bottom сферы
                            control.animationProgress.landingPosition = new Vector3(0f, c.point.y, c.point.z);

                            if (control.manualInput.enabled)
                            {
                                TestingSphere.transform.position = control.animationProgress.landingPosition;
                            }
                            return true;
                        }
                    }
                }
            }

            if (control.Rigid_Body.velocity.y < 0f)
            {
                foreach (GameObject o in control.collisionSpheres.bottomSpheres)
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
                            //control.animationProgress.ground = hit.collider.transform.root.gameObject; //что колайдерит bottom сферы
                            control.animationProgress.ground = hit.collider.transform.gameObject; //что колайдерит bottom сферы
                            control.animationProgress.landingPosition = new Vector3(0f, hit.point.y, hit.point.z);

                            if (control.manualInput.enabled)
                            {
                                TestingSphere.transform.position = control.animationProgress.landingPosition;
                            }
                            return true;
                        }
                    }
                }
            }
            control.animationProgress.ground = null;
            return false;
        }
    }
}
