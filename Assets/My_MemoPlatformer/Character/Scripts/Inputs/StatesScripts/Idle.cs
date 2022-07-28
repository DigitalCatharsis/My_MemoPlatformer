using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = " My_MemoPlatformer/AbilityData/Idle")]
    public class Idle : StateData
    {
        public override void UpdateAbility(CharacterState characterState, Animator animator)
        {
            if (VirtualInputManager.Instance.MoveRight)
            {
                animator.SetBool(TransitionParameter.Move.ToString(), true);
            }
            if (VirtualInputManager.Instance.MoveLeft)
            {
                animator.SetBool(TransitionParameter.Move.ToString(), true);
            }
        }
    }

}
