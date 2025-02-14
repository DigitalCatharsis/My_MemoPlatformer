using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/CheckRunningTurn")]
    public class CheckRunningTurn : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            {
                if (characterState.Rotation_Data.lockTurn)
                {
                    animator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Turn], false);
                    return;
                }

                if (characterState.Rotation_Data.IsFacingForward())
                {
                    if (characterState.characterControl.moveLeft)
                    {
                        animator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Turn], true);
                    }
                }

                if (!characterState.Rotation_Data.IsFacingForward())
                {
                    if (characterState.characterControl.moveRight)
                    {
                        animator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Turn], true);
                    }
                }
            }
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Turn], false);
        }


    }
}