using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/Jump")]
    public class Jump : CharacterAbility
    {
        [Range(0f, 1f)]
        [SerializeField] private float jumpTiming;
        [SerializeField] private float jumpForce;

        [Header("Extra Gravity")]
        [SerializeField] private bool canselPull;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.PlayerJump_Data.jumped = false;
            if (jumpTiming == 0f)
            {
                characterState.characterControl.RIGID_BODY.AddForce(Vector3.up * jumpForce);
                characterState.PlayerJump_Data.jumped = true;
            }

            characterState.VerticalVelocity_Data.noJumpCancel = canselPull;
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {   
            if (!characterState.PlayerJump_Data.jumped && stateInfo.normalizedTime >= jumpTiming)
            {
                characterState.characterControl.RIGID_BODY.AddForce(Vector3.up * jumpForce);
                characterState.PlayerJump_Data.jumped = true;
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
    }

}
