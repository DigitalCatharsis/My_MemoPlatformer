using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/Jump")]
    public class Jump : StateData
    {
        [Range(0f, 1f)]
        [SerializeField] private float jumpTiming;
        [SerializeField] private float jumpForce;
        [SerializeField] private AnimationCurve pull;


        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (jumpTiming ==0f)
            {
                CharacterControl control = characterState.GetCharacterControl(animator);
                control.Rigid_Body.AddForce(Vector3.up * jumpForce);
                control.animationProgress.jumped = true;
            }
            animator.SetBool(TransitionParameter.Grounded.ToString(), false);
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);
            control.pullMultipliyer = pull.Evaluate(stateInfo.normalizedTime);   //��� �������� ������ ������ ������������ �� �������!

            if (!control.animationProgress.jumped && stateInfo.normalizedTime >= jumpTiming)
            {
                control.Rigid_Body.AddForce(Vector3.up * jumpForce);
                control.animationProgress.jumped = true;
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);
            //control.pullMultipliyer = pull.Evaluate(stateInfo.normalizedTime);   //��� �������� ������ ������ ������������ �� �������!
            control.pullMultipliyer = 0f;
        }
    }

}
