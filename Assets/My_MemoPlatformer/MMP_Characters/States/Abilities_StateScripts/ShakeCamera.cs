using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/ShakeCamera")]
    public class ShakeCamera : StateData
    {
        [Range(0f, 0.99f)][SerializeField] private float _shakeTiming;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (_shakeTiming == 0f) 
            {
                CharacterControl control = characterState.GetCharacterControl(animator);
                CameraManager.Instance.ShakeCamera(0.2f);
                control.animationProgress.cameraShaken = true;
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);

            if (!control.animationProgress.cameraShaken)
            {
                if (stateInfo.normalizedTime >= _shakeTiming)
                {
                    control.animationProgress.cameraShaken = true;
                    CameraManager.Instance.ShakeCamera(0.2f);
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            CharacterControl control = characterState.GetCharacterControl(animator);
            control.animationProgress.cameraShaken = false;
        }
    }
}
