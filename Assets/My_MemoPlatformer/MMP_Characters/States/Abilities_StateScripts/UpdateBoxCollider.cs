using UnityEngine;

namespace My_MemoPlatformer
{

    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/UpdateBoxCollider")]
    public class UpdateBoxCollider : StateData
    {
        public Vector3 targetCenter;
        public float centerUpdateSpeed;
        [Space(10)]
        public Vector3 targetSize;
        public float sizeUpdateSpeed;

        private const string _landingState = "Jump_Normal_Landing";
        private const string _climbingState = "LedgeClimb";

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {      
            characterState.characterControl.animationProgress.targetSize = targetSize;
            characterState.characterControl.animationProgress.sizeSpeed = sizeUpdateSpeed;

            characterState.characterControl.animationProgress.targetCenter = targetCenter;
            characterState.characterControl.animationProgress.centerSpeed = centerUpdateSpeed;

            if (stateInfo.IsName(_landingState)) 
            {
                characterState.characterControl.animationProgress.isLanding = true;
            }

        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            //preventing pull back from platform when climbing cause of collider
            if (stateInfo.IsName(_climbingState))
            {
                if (stateInfo.normalizedTime > 0.7f)
                {
                    if (animator.GetBool(HashManager.Instance.dicMainParams[TransitionParameter.Grounded]) == true)
                    {
                        characterState.characterControl.animationProgress.isLanding = true;
                    }
                    else
                    {
                        characterState.characterControl.animationProgress.isLanding = false;
                    }
                }
                else
                {
                    characterState.characterControl.animationProgress.isLanding = false;
                }
            }
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (stateInfo.IsName(_landingState) || stateInfo.IsName(_climbingState))
            {
                characterState.characterControl.animationProgress.isLanding = false;
            }
        }
    }
}