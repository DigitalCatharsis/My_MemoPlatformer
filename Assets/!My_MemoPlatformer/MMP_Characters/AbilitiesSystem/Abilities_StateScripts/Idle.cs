using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/Idle")]
    public class Idle : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.Attack], false);
            animator.SetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.Move], false);

            characterState.PlayerRotation_Data.lockEarlyTurn = false;
            characterState.PlayerRotation_Data.lockDirectionNextState = false;
            characterState.BlockingObjData.ClearFrontBlockingObjDic();
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.PlayerRotation_Data.lockEarlyTurn = false;
            characterState.PlayerRotation_Data.lockDirectionNextState = false;

            if (characterState.characterControl.jump)
            {
                //do nothing
            }
            else
            {
                if (!characterState.PlayerAnimation_Data.IsRunning(typeof(Jump)))   //double update fix. Guess idle is overlapping jump or moveforward
                {
                    characterState.PlayerJump_Data.jumped = false;
                }
            }

            //Moving
            if (characterState.characterControl.moveLeft && characterState.characterControl.moveRight)
            {
                //nothing to fix bug with double press
            }
            else if (characterState.characterControl.moveRight)
            {
                animator.SetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.Move], true);
            }
            else if (characterState.characterControl.moveLeft)
            {
                animator.SetBool(HashManager.Instance.ArrMainParams[(int)MainParameterType.Move], true);
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            //animator.SetBool(TransitionParameter.Attack.ToString(), false);
        }
    }

}
