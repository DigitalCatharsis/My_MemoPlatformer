using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/FallPlatform")]
    public class FallPlatform : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {   
            if (characterState.characterControl.transform.position.z < characterState.characterControl.aiProgress.pathfindfingAgent.endSphere.transform.position.z)
            {
                characterState.characterControl.FaceForward(true);
            }
            else if (characterState.characterControl.transform.position.z > characterState.characterControl.aiProgress.pathfindfingAgent.endSphere.transform.position.z)
            {
                characterState.characterControl.FaceForward(false);
            }

            if (characterState.characterControl.aiProgress.AI_DistanceToStartSphere() > 3f)
            {
                characterState.characterControl.turbo = true;
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!characterState.characterControl.skinnedMeshAnimator.GetBool(TransitionParameter.Grounded.ToString()))
            {
                return;
            }


            if (characterState.characterControl.attack)
            {
                return;
            }

            if (characterState.characterControl.IsFacingForward())
            {
                if (characterState.characterControl.transform.position.z < characterState.characterControl.aiProgress.pathfindfingAgent.endSphere.transform.position.z)
                {
                    characterState.characterControl.moveRight = true;
                    characterState.characterControl.moveLeft = false;
                }
            }
            else
            {
                if (characterState.characterControl.transform.position.z > characterState.characterControl.aiProgress.pathfindfingAgent.endSphere.transform.position.z)
                {
                    characterState.characterControl.moveLeft = true;
                    characterState.characterControl.moveRight = false;
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }
    }

}