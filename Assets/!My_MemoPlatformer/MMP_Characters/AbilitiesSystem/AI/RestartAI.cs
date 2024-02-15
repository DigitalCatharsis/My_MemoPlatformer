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

            if (characterState.AI_CONTROLLER.RestartWalk())
            {
                characterState.AI_CONTROLLER.InitializeAI();
            }

            //fix from jumping after failed to ledgegrab
            if (characterState.Player_Animation_Data.IsRunning(typeof(Landing)))
            {
                characterState.characterControl.turbo = false;
                characterState.characterControl.jump = false;
                characterState.characterControl.moveUp = false;
                characterState.characterControl.moveLeft = false;
                characterState.characterControl.moveRight = false;
                characterState.characterControl.moveDown = false;
                characterState.characterControl.aiController.InitializeAI();
            }

            if (characterState.AI_CONTROLLER.IsAttacking())
            {
                if (characterState.characterControl.aiProgress.AIDistanceToTarget() > 3f ||
                    !characterState.characterControl.aiProgress.TargetIsOnTheSamePlatform())
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

            // path is blocked
            if (characterState.BlockingObj_Data.frontBlockingDictionaryCount == 0)
            {
                characterState.characterControl.aiProgress.blockingCharacter = null;
            }
            else
            {
                List<GameObject> objs = characterState.BlockingObj_Data.GetFrontBlockingCharactersList();

                foreach (GameObject o in objs)
                {
                    CharacterControl blockingChar = CharacterManager.Instance.GetCharacter(o);

                    if (blockingChar != null)
                    {
                        characterState.characterControl.aiProgress.blockingCharacter = blockingChar;
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
                if (characterState.Ground_Data.ground != null)
                {
                    if (!characterState.Player_Animation_Data.IsRunning(typeof(Jump)) &&
                        !characterState.Player_Animation_Data.IsRunning(typeof(JumpPrep)))
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

            //startsphere height
            if (characterState.Ground_Data.ground != null &&
                !characterState.Player_Animation_Data.IsRunning(typeof(Jump)) &&
                !characterState.Player_Animation_Data.IsRunning(typeof(WallJump_Prep)))
            {
                var height = characterState.characterControl.aiProgress.GetStartSphereHeight();
                if (height > 0.1f)
                {
                    characterState.characterControl.turbo = false;
                    characterState.characterControl.jump = false;
                    characterState.characterControl.moveUp = false;
                    characterState.characterControl.moveLeft = false;
                    characterState.characterControl.moveRight = false;
                    characterState.characterControl.moveDown = false;
                    characterState.characterControl.aiController.InitializeAI();
                }
                ////sometimes AI doing nothing while control.attack is true
                else if (characterState.characterControl.aiProgress.TargetIsOnTheSamePlatform() && characterState.characterControl.attack)
                {
                    characterState.characterControl.attack = false;
                    characterState.characterControl.turbo = false;
                    characterState.characterControl.jump = false;
                    characterState.characterControl.moveUp = false;
                    characterState.characterControl.moveLeft = false;
                    characterState.characterControl.moveRight = false;
                    characterState.characterControl.moveDown = false;
                    characterState.characterControl.aiController.InitializeAI();
                }
            }

            ////sometimes AI doing nothing while control.attack is true
            //if (characterState.Ground_Data.ground != null &&
            //    !characterState.Animation_Data.IsRunning(typeof(Jump)) &&
            //    !characterState.Animation_Data.IsRunning(typeof(WallJump_Prep)) &&
            //     characterState.characterControl.attack)
            //{
            //    if (characterState.characterControl.aiProgress.GetStartSphereHeight() == characterState.characterControl.aiProgress.GetEndSphereHeight())
            //    {
            //        characterState.characterControl.turbo = false;
            //        characterState.characterControl.jump = false;
            //        characterState.characterControl.moveUp = false;
            //        characterState.characterControl.moveLeft = false;
            //        characterState.characterControl.moveRight = false;
            //        characterState.characterControl.moveDown = false;
            //        characterState.characterControl.aiController.InitializeAI();
            //    }
            //}
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        private bool AIIsOnGround(CharacterControl control)
        {
            if (control.PLAYER_ANIMATION_DATA.IsRunning(typeof(MoveUp)))
            {
                return false;
            }

            if (control.rigidBody.useGravity)
            {
                if (control.skinnedMeshAnimator.GetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
