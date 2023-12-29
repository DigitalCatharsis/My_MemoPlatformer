using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/ShakeCamera")]
    public class ShakeCamera : CharacterAbility
    {
        [Range(0f, 0.99f)][SerializeField] private float _shakeTiming;
        public float shakeLength;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (_shakeTiming == 0f) 
            {                
                CameraManager.Instance.ShakeCamera(shakeLength);
                characterState.characterControl.animationProgress.cameraShaken = true;
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {           
            if (!characterState.characterControl.animationProgress.cameraShaken)
            {
                if (stateInfo.normalizedTime >= _shakeTiming)
                {
                    characterState.characterControl.animationProgress.cameraShaken = true;
                    CameraManager.Instance.ShakeCamera(shakeLength);
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {            
            characterState.characterControl.animationProgress.cameraShaken = false;
        }
    }
}
