using UnityEngine;
using UnityEngine.Rendering;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/UpdateBoxCollider")]
    public class UpdateBoxCollider : CharacterAbility
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
            if (DebugContainer_Data.Instance.debug_Colliders)
            {
                Debug.Log("Entered to UpdateBoxCollider OnEnter");
                Debug.Log("Setting values -> BoxColliderData");
                Debug.Log($"<color=Red> Current State:{characterState.Animation_Data.currentState}</color>");
            }
            characterState.BoxCollider_Data.targetSize = targetSize;
            characterState.BoxCollider_Data.size_Update_Speed = sizeUpdateSpeed;

            characterState.BoxCollider_Data.targetCenter = targetCenter;
            characterState.BoxCollider_Data.center_Update_Speed = centerUpdateSpeed;

            if (stateInfo.IsName(_landingState))
            {
                characterState.BoxCollider_Data.isLanding = true;
            }

        }
        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            //preventing pull back from platform when climbing cause of collider
            if (stateInfo.IsName(_climbingState))
            {
                if (stateInfo.normalizedTime > 0.7f)
                {
                    if (animator.GetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]) == true)
                    {
                        characterState.BoxCollider_Data.isLanding = true;
                    }
                    else
                    {
                        characterState.BoxCollider_Data.isLanding = false;
                    }
                }
                else
                {
                    characterState.BoxCollider_Data.isLanding = false;
                }
            }
        }
        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (DebugContainer_Data.Instance.debug_Colliders)
            {
                Debug.Log("Finished updateing BoxCollider State");
            }
            if (stateInfo.IsName(_landingState) || stateInfo.IsName(_climbingState))
            {
                characterState.BoxCollider_Data.isLanding = false;
            }
        }
    }
}