using Unity.Burst.CompilerServices;
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
                animator.SetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.Grounded], true);
            }
            else
            {
                animator.SetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.Grounded], false);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        bool IsGrounded(CharacterControl control)
        {
            if (control.PlayerGround_Data.BoxColliderContacts != null)
            {
                foreach (var c in control.PlayerGround_Data.BoxColliderContacts)
                {
                    var colliderBottom = (control.transform.position.y + control.boxCollider.center.y) - (control.boxCollider.size.y / 2f);
                    var yDiffirence = Mathf.Abs(c.point.y - colliderBottom);

                    if (yDiffirence < 0.01f)
                    {
                        if (Mathf.Abs(control.Rigid_Body.velocity.y) < 0.001f)
                        {
                            //control.animationProgress.ground = c.otherCollider.transform.root.gameObject; //��� ���������� bottom �����
                            control.PlayerGround_Data.ground = c.otherCollider.transform.gameObject; //��� ���������� bottom �����
                            control.BoxCollider_Data.landingPosition = new Vector3(0f, c.point.y, c.point.z);
                            return true;
                        }
                    }
                }
            }

            if (control.Rigid_Body.velocity.y < 0f)
            {
                foreach (GameObject o in control.CollisionSpheres_Data.bottomSpheres)
                {
                    var blockingObj = CollisionDetection.GetCollidingObject(control, o, -Vector3.up, distance, ref control.BlockingObj_Data.raycastContactPoint);

                    if (blockingObj != null)
                    {
                        var c = CharacterManager.Instance.GetCharacter(blockingObj.transform.root.gameObject);

                        if (c == null)
                        {
                            control.PlayerGround_Data.ground = blockingObj.transform.root.gameObject; //��� ���������� bottom �����
                            control.BoxCollider_Data.landingPosition = new Vector3(0f, control.BlockingObj_Data.raycastContactPoint.y, control.BlockingObj_Data.raycastContactPoint.z);
                            return true;
                        }
                    }
                }
            }
            control.PlayerGround_Data.ground = null;
            return false;
        }
    }
}