using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/ToggleGravity")]
    public class ToggleGravity : StateData
    {
        public bool on;
        public bool onStart;
        public bool onEnd;
        [Tooltip("Toggle Gravity at this % of animation")] public float customTiming;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (onStart)
            {
                
                ToggleGrav(characterState.characterControl);
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (customTiming != 0f)
            {
                if (customTiming <= stateInfo.normalizedTime)
                {
                    ToggleGrav(characterState.characterControl);
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (onEnd)
            {                
                ToggleGrav(characterState.characterControl);
            }   
        }

        private void ToggleGrav(CharacterControl control)
        {
            control.Rigid_Body.velocity = Vector3.zero;
            control.Rigid_Body.useGravity = on;  //ON not qual True xD
        }
    }
}
