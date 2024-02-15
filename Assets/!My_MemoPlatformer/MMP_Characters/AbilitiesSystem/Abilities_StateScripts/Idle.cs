using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/Idle")]
    public class Idle : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Move], false);

            characterState.BlockingObj_Data.ClearFrontBlockingObjDic();
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (characterState.characterControl.jump)
            {
                //do nothing
            }
            else
            {
                if (!characterState.Player_Animation_Data.IsRunning(typeof(Jump)))   //double update fix. Guess idle is overlapping jump or moveforward
                {
                    characterState.Jump_Data.dicJumped.Clear();
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }
    }

}
