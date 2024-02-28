using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/RestartAI")]
    public class RestartAI : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.AICONTROLLER_DATA.aiStatus = Ai_Status.Restarting_AI.ToString();
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!characterState.characterControl.AICONTROLLER_DATA.aIConditions.AIIsOnGround(characterState.characterControl))
            {
                return;
            }

            //Walk again
            if (characterState.AI_CONTROLLER_DATA.aIConditions.IsRestartWalkCondition())
            {
                characterState.AI_CONTROLLER_DATA.InitializeAI();
                characterState.characterControl.AICONTROLLER_DATA.aIBehavior.StartAi();
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

                characterState.characterControl.AICONTROLLER_DATA.InitializeAI();
                characterState.characterControl.AICONTROLLER_DATA.aIBehavior.StartAi();
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
                    characterState.characterControl.AICONTROLLER_DATA.aIBehavior.StartAi();
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
                        characterState.characterControl.AICONTROLLER_DATA.aIBehavior.StartAi();
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
                    characterState.characterControl.AICONTROLLER_DATA.aIBehavior.StartAi();
                }
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }
    }
}
