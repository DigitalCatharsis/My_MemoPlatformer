using UnityEditor;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/MoveUp")]
    public class MoveUp : StateData
    {
        public AnimationCurve speedGraph;
        public float speed;


        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.animationProgress.latestMoveUpScript = this;
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!characterState.characterControl.Rigid_Body.useGravity)
            {
                if (characterState.characterControl.animationProgress.upBlockingObjects.Count == 0)
                {
                    characterState.characterControl.transform.Translate(Vector3.up * speed * speedGraph.Evaluate(stateInfo.normalizedTime) * Time.deltaTime);
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }
    }
}
