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
        [Header("Extra Gravity")]
        [SerializeField] private AnimationCurve pull;
        [SerializeField] private bool canselPull;



        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);
            control.animationProgress.jumped = false;
            if (jumpTiming == 0f)
            {
                control.Rigid_Body.AddForce(Vector3.up * jumpForce);
                control.animationProgress.jumped = true;
            }

            control.animationProgress.cancelPull = canselPull;
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
