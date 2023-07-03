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
            
            characterState.characterControl.animationProgress.jumped = false;
            if (jumpTiming == 0f)
            {
                characterState.characterControl.Rigid_Body.AddForce(Vector3.up * jumpForce);
                characterState.characterControl.animationProgress.jumped = true;
            }

            characterState.characterControl.animationProgress.cancelPull = canselPull;
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
            characterState.characterControl.pullMultipliyer = pull.Evaluate(stateInfo.normalizedTime);   //ÒÓÒ ÃĞÅÁÀÍÀß ÂÛÑÎÒÀ ÏĞÛÆÊÀ ÂÇÀÂÈÑÈÌÎÑÒÈ ÎÒ ÍÀÆÀÒÈß!

            if (!characterState.characterControl.animationProgress.jumped && stateInfo.normalizedTime >= jumpTiming)
            {
                characterState.characterControl.Rigid_Body.AddForce(Vector3.up * jumpForce);
                characterState.characterControl.animationProgress.jumped = true;
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {            
            //control.pullMultipliyer = pull.Evaluate(stateInfo.normalizedTime);   //ÒÓÒ ÃĞÅÁÀÍÀß äëèííà ÏĞÛÆÊÀ ÂÇÀÂÈÑÈÌÎÑÒÈ ÎÒ ÍÀÆÀÒÈß!
            characterState.characterControl.pullMultipliyer = 0f;
        }
    }

}
