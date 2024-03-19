using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/ChecMovement")]
    public class CheckMovement : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            Check_OnRunToTurn_LeftRightUpDown(characterState.characterControl);

            if (characterState.characterControl.moveLeft || characterState.characterControl.moveRight)
            {
                animator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Move], true);
            }
            else
            {
                animator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Move], false);
            }

            if (characterState.characterControl.turbo)
            {
                animator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Turbo], true);
            }
            else
            {
                animator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Turbo], false);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.skinnedMeshAnimator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.RunToTurn_Left], false);
            characterState.characterControl.skinnedMeshAnimator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.RunToTurn_Right], false);
            characterState.characterControl.skinnedMeshAnimator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.RunToTurn_Up], false);
            characterState.characterControl.skinnedMeshAnimator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.RunToTurn_Down], false);
        }

        void Check_OnRunToTurn_LeftRightUpDown(CharacterControl control)
        {
            if (control.moveLeft)
            {
                control.skinnedMeshAnimator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.RunToTurn_Left], true);
            }
            else
            {
                control.skinnedMeshAnimator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.RunToTurn_Left], false);
            }

            if (control.moveRight)
            {
                control.skinnedMeshAnimator.
                    SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.RunToTurn_Right], true);
            }
            else
            {
                control.skinnedMeshAnimator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.RunToTurn_Right], false);
            }

            if (control.moveUp)
            {
                control.skinnedMeshAnimator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.RunToTurn_Up], true);
            }
            else
            {
                control.skinnedMeshAnimator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.RunToTurn_Up], false);
            }

            if (control.moveDown)
            {
                control.skinnedMeshAnimator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.RunToTurn_Down], true);
            }
            else
            {
                control.skinnedMeshAnimator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.RunToTurn_Down], false);
            }
        }
    }
}
