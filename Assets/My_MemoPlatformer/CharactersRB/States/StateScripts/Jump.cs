using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/Jump")]
    public class Jump : StateData
    {
        [SerializeField] private float jumpForce;
        [SerializeField] private AnimationCurve gravity; 
        [SerializeField] private AnimationCurve pull; 

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.GetCharacterControl(animator).Rigid_Body.AddForce(Vector3.up * jumpForce);
            animator.SetBool(TransitionParameter.Grounded.ToString(), false);
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);

            control.gravityMultipliyer = gravity.Evaluate(stateInfo.normalizedTime);
            control.pullMultipliyer = pull.Evaluate(stateInfo.normalizedTime);   //“”“ √–≈¡¿Õ¿ﬂ ¬€—Œ“¿ œ–€∆ ¿ ¬«¿¬»—»ÃŒ—“» Œ“ Õ¿∆¿“»ﬂ!
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }
    }

}
