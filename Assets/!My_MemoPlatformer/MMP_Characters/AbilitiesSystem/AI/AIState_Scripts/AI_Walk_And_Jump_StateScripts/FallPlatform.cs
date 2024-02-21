using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/FallPlatform")]
    public class FallPlatform : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {   

        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!characterState.characterControl.skinnedMeshAnimator.GetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]))
            {
                return;
            }

            if (characterState.characterControl.attack)
            {
                return;
            }

            if (characterState.characterControl.transform.position.z < characterState.characterControl.AICONTROLLER_DATA.pathfindingAgent.endSphere.transform.position.z)
            {
                characterState.characterControl.moveRight = true;
                characterState.characterControl.moveLeft = false;
            }
            else if (characterState.characterControl.transform.position.z > characterState.characterControl.AICONTROLLER_DATA.pathfindingAgent.endSphere.transform.position.z)
            {
                characterState.characterControl.moveRight = false;
                characterState.characterControl.moveLeft = true;
            }

            if (characterState.characterControl.AICONTROLLER_DATA.aiLogistic.AIDistanceToStartSphere() > 3f)
            {
                characterState.characterControl.turbo = true;
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }
    }

}