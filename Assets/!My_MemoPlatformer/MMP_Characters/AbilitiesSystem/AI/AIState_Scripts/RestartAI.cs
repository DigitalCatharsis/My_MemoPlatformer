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

            if (characterState.AI_CONTROLLER_DATA.aIBehavior.IsRestartWalkCondition())
            {
                //TODO: ?????????????
                characterState.AI_CONTROLLER_DATA.InitializeAI();
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
                //TODO: ??????????????????
                characterState.characterControl.AICONTROLLER_DATA.InitializeAI();
            }

            if (characterState.AI_CONTROLLER_DATA.aIConditions.IsInAttackingAnimation())
            {
                if (characterState.characterControl.AICONTROLLER_DATA.aiLogistic.AIDistanceToTarget() > 3f ||
                    !characterState.characterControl.AICONTROLLER_DATA.aIConditions.TargetIsOnTheSamePlatform())
                {
                    characterState.characterControl.turbo = false;
                    characterState.characterControl.jump = false;
                    characterState.characterControl.moveUp = false;
                    characterState.characterControl.moveLeft = false;
                    characterState.characterControl.moveRight = false;
                    characterState.characterControl.moveDown = false;
                    characterState.characterControl.AICONTROLLER_DATA.InitializeAI();
                }
            }

            // path is blocked
            if (characterState.BlockingObj_Data.frontBlockingDictionaryCount == 0)
            {
                characterState.characterControl.AICONTROLLER_DATA.blockingCharacter = null;
            }
            else
            {
                List<GameObject> objs = characterState.BlockingObj_Data.GetFrontBlockingCharactersList();

                foreach (GameObject o in objs)
                {
                    CharacterControl blockingChar = CharacterManager.Instance.GetCharacter(o);

                    if (blockingChar != null)
                    {
                        characterState.characterControl.AICONTROLLER_DATA.blockingCharacter = blockingChar;
                        break;
                    }
                    else
                    {
                        characterState.characterControl.AICONTROLLER_DATA.blockingCharacter = null;
                    }
                }
            }

            if (characterState.characterControl.AICONTROLLER_DATA.blockingCharacter != null)
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
                        characterState.characterControl.AICONTROLLER_DATA.InitializeAI();
                    }
                }
            }

            //startsphere height
            if (characterState.Ground_Data.ground != null &&
                !characterState.Player_Animation_Data.IsRunning(typeof(Jump)) &&
                !characterState.Player_Animation_Data.IsRunning(typeof(WallJump_Prep)))
            {
                var height = characterState.characterControl.AICONTROLLER_DATA.aiLogistic.GetStartSphereHeight();
                if (height > 0.1f)
                {
                    characterState.characterControl.turbo = false;
                    characterState.characterControl.jump = false;
                    characterState.characterControl.moveUp = false;
                    characterState.characterControl.moveLeft = false;
                    characterState.characterControl.moveRight = false;
                    characterState.characterControl.moveDown = false;
                    characterState.characterControl.AICONTROLLER_DATA.InitializeAI();
                }
                ////sometimes AI doing nothing while control.attack is true
                else if (characterState.characterControl.AICONTROLLER_DATA.aIConditions.TargetIsOnTheSamePlatform() && characterState.characterControl.attack)
                {
                    characterState.characterControl.attack = false;
                    characterState.characterControl.turbo = false;
                    characterState.characterControl.jump = false;
                    characterState.characterControl.moveUp = false;
                    characterState.characterControl.moveLeft = false;
                    characterState.characterControl.moveRight = false;
                    characterState.characterControl.moveDown = false;
                    characterState.characterControl.AICONTROLLER_DATA.InitializeAI();
                }
            }

            ////sometimes AI doing nothing while control.attack is true
            //if (characterState.Ground_Data.ground != null &&
            //    !characterState.Animation_Data.IsRunning(typeof(Jump)) &&
            //    !characterState.Animation_Data.IsRunning(typeof(WallJump_Prep)) &&
            //     characterState.characterControl.attack)
            //{
            //    if (characterState.characterControl.AICONTROLLER_DATA.GetStartSphereHeight() == characterState.characterControl.AICONTROLLER_DATA.GetEndSphereHeight())
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
