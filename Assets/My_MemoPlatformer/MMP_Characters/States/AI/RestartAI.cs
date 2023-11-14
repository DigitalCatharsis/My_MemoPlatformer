using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/RestartAI")]
    public class RestartAI : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            //walking
            if (characterState.characterControl.aiProgress.AI_DistanceToEndSphere() < 1.0f)
            {
                if (characterState.characterControl.aiProgress.TargetDistanceToEndSphere() > 0.5f)
                {
                    if (characterState.characterControl.aiProgress.TargetIsGrounded())
                    {
                        characterState.characterControl.aiController.InitializeAI();
                    }
                }
            }

            //landing
            if (characterState.characterControl.animationProgress.IsRunning(typeof(Landing)))
            {
                characterState.characterControl.turbo = false;
                characterState.characterControl.jump = false;
                characterState.characterControl.moveUp = false;
                characterState.characterControl.aiController.InitializeAI();
            }

            //path is blocked
            characterState.characterControl.aiProgress.blockingCharacter = CharacterManager.Instance.GetCharacter(characterState.characterControl.animationProgress.blockingObj);

            if (characterState.characterControl.aiProgress.blockingCharacter != null)
            {
                if (characterState.characterControl.animationProgress.ground != null)
                {
                    if (characterState.characterControl.animationProgress.ground != null)
                    {
                        if (!characterState.characterControl.animationProgress.IsRunning(typeof(Jump)) 
                            && !characterState.characterControl.animationProgress.IsRunning(typeof(JumpPrep)))
                            {
                            characterState.characterControl.turbo = false;
                            characterState.characterControl.jump = false;
                            characterState.characterControl.moveUp = false;
                            characterState.characterControl.moveLeft = false;
                            characterState.characterControl.moveRight = false;
                            characterState.characterControl.moveDown = false;
                            characterState.characterControl.aiController.InitializeAI();
                        }
                    }
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
    }
}
