using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/Jump")]
    public class Jump : CharacterAbility
    {
        public int jumpIndex;

        [Range(0f, 1f)]
        public float jumpTiming;
        public float jumpForce;
        public bool clearPreviousVelocity;

        [Header("Extra Gravity")]
        public bool cancelPull;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!characterState.Jump_Data.dicJumped.ContainsKey(jumpIndex))
            {
                characterState.Jump_Data.dicJumped.Add(jumpIndex, false);
            }

            characterState.Vertical_Velocity_Data.noJumpCancel = cancelPull;

            if (jumpTiming == 0f)
            {
                MakeJump(characterState.characterControl);
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!characterState.Jump_Data.dicJumped[jumpIndex] && stateInfo.normalizedTime >= jumpTiming)
            {
                MakeJump(characterState.characterControl);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.JUMP_DATA.dicJumped[jumpIndex] = false;
        }

        void MakeJump(CharacterControl control)
        {
            if (DebugContainer_Data.Instance.debug_Jump)
            {
                Debug.Log("Making jump: " + this.name);
            }

            if (control.JUMP_DATA.dicJumped[jumpIndex])
            {
                if (DebugContainer_Data.Instance.debug_Jump)
                {
                    Debug.Log("Preventing double jump");
                }
                return;
            }

            if (clearPreviousVelocity)
            {
                control.rigidBody.velocity = Vector3.zero;
            }

            // automatically turn gravity on before jumping
            if (!control.rigidBody.useGravity)
            {
                control.rigidBody.useGravity = true;
            }

            control.rigidBody.AddForce(Vector3.up * jumpForce);
            control.JUMP_DATA.dicJumped[jumpIndex] = true;
        }
    }

}
