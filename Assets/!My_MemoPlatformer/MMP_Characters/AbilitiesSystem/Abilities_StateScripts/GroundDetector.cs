using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/GroundDetector")]
    public class GroundDetector : CharacterAbility
    {
        [SerializeField] private float distance;
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (IsGrounded(characterState.characterControl))
            {
                animator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded], true);
            }
            else
            {
                animator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded], false);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        bool IsGrounded(CharacterControl control)
        {
            if (control.GROUND_DATA.BoxColliderContacts != null)
            {
                foreach (var c in control.GROUND_DATA.BoxColliderContacts)
                {
                    var colliderBottom = (control.transform.position.y + control.boxCollider.center.y) - (control.boxCollider.size.y / 2f);
                    var yDiffirence = Mathf.Abs(c.point.y - colliderBottom);

                    if (yDiffirence < 0.01f)
                    {
                        if (Mathf.Abs(control.RIGID_BODY.velocity.y) < 0.001f)
                        {
                            //control.animationProgress.ground = c.otherCollider.transform.root.gameObject; //что колайдерит bottom сферы
                            control.GROUND_DATA.ground = c.otherCollider.transform.gameObject; //что колайдерит bottom сферы
                            control.BOX_COLLIDER_DATA.landingPosition = new Vector3(0f, c.point.y, c.point.z);
                            return true;
                        }
                    }
                }
            }

            if (control.RIGID_BODY.velocity.y < 0f)
            {
                foreach (GameObject o in control.COLLISION_SPHERE_DATA.bottomSpheres)
                {
                    var blockingObj = CollisionDetection.GetCollidingObject
                        (control, o, -Vector3.up, distance, ref control.BLOCKING_OBJ_DATA.raycastContactPoint);

                    if (blockingObj != null)
                    {
                        var c = CharacterManager.Instance.GetCharacter(blockingObj.transform.root.gameObject);

                        if (c == null)
                        {
                            control.GROUND_DATA.ground = blockingObj.transform.root.gameObject; //что колайдерит bottom сферы
                            control.BOX_COLLIDER_DATA.landingPosition = new Vector3(
                                0f, 
                                control.BLOCKING_OBJ_DATA.raycastContactPoint.y, 
                                control.BLOCKING_OBJ_DATA.raycastContactPoint.z);

                            return true;
                        }
                    }
                }
            }
            control.GROUND_DATA.ground = null;
            return false;
        }
    }
}
