using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/AIAttack")]
    public class AIAttack : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.turbo = false;
            characterState.characterControl.attack = false;
        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!characterState.characterControl.aiProgress.TargetIsDead())
            {
                characterState.characterControl.aiProgress.DoAttack();
            }
            else
            {
                characterState.characterControl.moveRight = false;
                characterState.characterControl.moveLeft = false;
                characterState.characterControl.attack = false;
            }
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
    }
}


