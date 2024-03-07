using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AI/StartWalking")]
    public class StartWalking : CharacterAbility
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.AICONTROLLER_DATA.aiStatus = Ai_Status.Starting_To_Walk.ToString();
            characterState.characterControl.AICONTROLLER_DATA.aIAttacks.SetRandomFlyingKick();
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (characterState.characterControl.attack) 
            {
                return;
            }

            //Jumping
            if (characterState.characterControl.AICONTROLLER_DATA.aIConditions.EndSphereIsHigherThanStartSphere())
            {
                if (characterState.characterControl.AICONTROLLER_DATA.aiLogistic.AIDistanceToStartSphere() < 0.08f) //how close are we to the checkpoint    //Здесь часто бывает баг (когда иди бегает вокруг Start Point) из-за разных смещений платформы или ИИ относительно друг друга. Увелич да < 0.1f для дебага
                {
                    characterState.characterControl.moveLeft = false;
                    characterState.characterControl.moveRight = false;

                    animator.SetBool(HashManager.Instance.arrAITransitionParams[(int)AI_Transition.Jump_Platform], true);
                    return;
                }
            }

            //fall
            if (characterState.characterControl.AICONTROLLER_DATA.aIConditions.EndSphereIsLowerThanStartSphere())
            {
                characterState.characterControl.AICONTROLLER_DATA.aIBehavior.WalkStraightToTheEndSphere();

                animator.SetBool(HashManager.Instance.arrAITransitionParams[(int)AI_Transition.Fall_Platform], true);
                return;
            }

            //straight
            if (characterState.characterControl.AICONTROLLER_DATA.aiLogistic.AIDistanceToStartSphere() > 1.5f)
            {
                characterState.characterControl.turbo = true;
            }
            else
            {
                characterState.characterControl.turbo = false;
            }
            characterState.characterControl.AICONTROLLER_DATA.aIBehavior.MoveToTheStartSphere();

            if (characterState.characterControl.AICONTROLLER_DATA.aiLogistic.AIDistanceToEndSphere() < 1f)
            {
                characterState.characterControl.turbo = false;
                characterState.characterControl.moveLeft = false;
                characterState.characterControl.moveRight = false;
            }

            if (characterState.characterControl.AICONTROLLER_DATA.aIConditions.TargetIsOnTheSamePlatform())
            {
                characterState.characterControl.AICONTROLLER_DATA.aIBehavior.RepositionPESpheresDestination();
            }
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            animator.SetBool(HashManager.Instance.arrAITransitionParams[(int)AI_Transition.Jump_Platform], false);
            animator.SetBool(HashManager.Instance.arrAITransitionParams[(int)AI_Transition.Fall_Platform], false);
        }
    }
}
