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
        //[SerializeField] private AnimationCurve gravity; 
        [SerializeField] private AnimationCurve pull;
        private bool isJumped;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (jumpTiming ==0f)
            {
                characterState.GetCharacterControl(animator).Rigid_Body.AddForce(Vector3.up * jumpForce);
                isJumped = true;
            }
            animator.SetBool(TransitionParameter.Grounded.ToString(), false);
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);

           // control.gravityMultipliyer = gravity.Evaluate(stateInfo.normalizedTime);
            control.pullMultipliyer = pull.Evaluate(stateInfo.normalizedTime);   //“”“ √–≈¡¿Õ¿ﬂ ¬€—Œ“¿ œ–€∆ ¿ ¬«¿¬»—»ÃŒ—“» Œ“ Õ¿∆¿“»ﬂ!

            if (!isJumped && stateInfo.normalizedTime >= jumpTiming)
            {
                characterState.GetCharacterControl(animator).Rigid_Body.AddForce(Vector3.up * jumpForce);
                isJumped = true;
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);

            control.pullMultipliyer = pull.Evaluate(stateInfo.normalizedTime);   //“”“ √–≈¡¿Õ¿ﬂ ¬€—Œ“¿ œ–€∆ ¿ ¬«¿¬»—»ÃŒ—“» Œ“ Õ¿∆¿“»ﬂ!
            control.pullMultipliyer = 0f;
            isJumped=false;
        }
    }

}
