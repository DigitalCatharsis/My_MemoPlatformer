using My_MemoPlatformer.Datasets;
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
        //[SerializeField] private AnimationCurve pull;
        [SerializeField] private bool canselPull;



        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.Air_Control.SetBool((int)AirControlBool.JUMPED, false);
            if (jumpTiming == 0f)
            {
                characterState.characterControl.Rigid_Body.AddForce(Vector3.up * jumpForce);
                characterState.characterControl.Air_Control.SetBool((int)AirControlBool.JUMPED, true);
            }

            characterState.characterControl.Air_Control.SetBool((int)AirControlBool.CANCEL_PULL, canselPull);
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {            
            //characterState.characterControl.pullMultipliyer = pull.Evaluate(stateInfo.normalizedTime);   //ÒÓÒ ÃÐÅÁÀÍÀß ÂÛÑÎÒÀ ÏÐÛÆÊÀ ÂÇÀÂÈÑÈÌÎÑÒÈ ÎÒ ÍÀÆÀÒÈß!
            var jumped = characterState.characterControl.Air_Control.GetBool((int)AirControlBool.JUMPED);

            if (!jumped && stateInfo.normalizedTime >= jumpTiming)
            {
                characterState.characterControl.Rigid_Body.AddForce(Vector3.up * jumpForce);
                characterState.characterControl.Air_Control.SetBool((int)AirControlBool.JUMPED, true);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
    }

}
