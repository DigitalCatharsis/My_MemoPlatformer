using UnityEngine;

namespace My_MemoPlatformer
{
    public abstract class CharacterAbility : ScriptableObject
    {
        public abstract void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo);
        public abstract void OnEnter(CharacterState characterState, Animator animator,AnimatorStateInfo stateInfo);
        public abstract void OnExit(CharacterState characterState, Animator animator,AnimatorStateInfo stateInfo);
    }
}