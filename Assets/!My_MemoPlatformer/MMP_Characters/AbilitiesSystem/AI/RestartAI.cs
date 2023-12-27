using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/RestartAI")]
    public class RestartAI : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!AIIsOnGround(characterState.characterControl))
            {
                return;
            }

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
            if (characterState.PlayerAnimation_Data.IsRunning(typeof(Landing)))
            {
                characterState.characterControl.turbo = false;
                characterState.characterControl.jump = false;
                characterState.characterControl.moveUp = false;
                characterState.characterControl.aiController.InitializeAI();
            }

            //path is blocked
            if (characterState.BlockingObjData.frontBlockingDictionaryCount == 0)
            {
                characterState.characterControl.aiProgress.blockingCharacter = null;
            }
            else
            {
                var objs = characterState.characterControl.BlockingObj_Data.GetFrontBlockingCharactersList();

                foreach (var o in objs)
                {
                    CharacterControl blockingCharacter = CharacterManager.Instance.GetCharacter(o);
                     
                    if (blockingCharacter != null)
                    {
                        characterState.characterControl.aiProgress.blockingCharacter = blockingCharacter;
                        break;
                    }
                    else
                    {
                        characterState.characterControl.aiProgress.blockingCharacter = null;
                    }
                }
            }
            
            if (characterState.characterControl.aiProgress.blockingCharacter != null)
            {
                if (characterState.characterControl.PlayerGround_Data.ground != null)
                {
                    if (characterState.characterControl.PlayerGround_Data.ground != null)
                    {
                        if (!characterState.PlayerAnimation_Data.IsRunning(typeof(Jump))
                            && !characterState.PlayerAnimation_Data.IsRunning(typeof(JumpPrep)))
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

            //startsphere Height
            if (characterState.characterControl.PlayerGround_Data.ground != null
                && !characterState.PlayerAnimation_Data.IsRunning(typeof(Jump))
                && !characterState.PlayerAnimation_Data.IsRunning(typeof(WallJump_Prep)))
            {
                if (characterState.characterControl.aiProgress.GetStartSphereHeight() > 0.1f)
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

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        private bool AIIsOnGround(CharacterControl control)
        {
            if (control.PlayerAnimation_Data.IsRunning(typeof(MoveUp)))
            {
                return false;
            }

            if (control.Rigid_Body.useGravity)
            {
                if (control.skinnedMeshAnimator.GetBool(HashManager.Instance.dicMainParams[TransitionParameter.Grounded]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
