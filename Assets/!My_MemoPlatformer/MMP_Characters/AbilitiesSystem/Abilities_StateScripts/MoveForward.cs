using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/MoveForward")]
    public class MoveForward : CharacterAbility
    {
        [Tooltip("Prevent turning when running from idle")] 
        public bool allowEarlyTurn;
        public bool lockDirection;
        [Tooltip("Move no matter what")] 
        public bool constant;
        public AnimationCurve speedGraph;
        public float Speed;
        [Tooltip("Distance to prevent moving, \n MAKE SURE ITS LESS THAN ColliderEdge RADIUS!")] 
        public float blockDistance;

        [Header("IgnoreCharacterBox")]
        public bool ignoreCharacterBox;
        public float ignoreStartTime;
        public float ignoreEndTime;

        [Header("Momentum")]
        public bool useMomentum;
        public float startingMomentum;
        public float maxMomentum;
        public bool StartFromPreviousMomentum;
        public bool clearMomentumOnExit;

        [Header("MoveOnHit")]
        [Tooltip("Move to direction away from Attacker")] 
        public bool moveOnHit;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.animationProgress.latestMoveForwardScript = this; //При переходе между стейтами moveForward предыдущего
                                                                                              //стейта может не успеть отработать и вызвать баги

            if (allowEarlyTurn)
            {
                // early turn can be locked by previous states
                if (characterState.characterControl.moveLeft)
                {
                    characterState.Rotation_Data.FaceForward(false);
                }
                if (characterState.characterControl.moveRight)
                {
                    characterState.Rotation_Data.FaceForward(true);
                }
            }

            if (!StartFromPreviousMomentum)
            {
                if (startingMomentum > 0.001f)
                {
                    if (characterState.Rotation_Data.IsFacingForward())
                    {
                        characterState.MomentumCalculator_Data.momentum = startingMomentum;
                    }
                    else
                    {
                        characterState.MomentumCalculator_Data.momentum = -startingMomentum;
                    }
                }
            }
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (DebugContainer_Data.Instance.debug_MoveForward)
            {
                Debug.Log(stateInfo.normalizedTime);
            }

            if (characterState.characterControl.animationProgress.latestMoveForwardScript != this)
            {
                return;
            }

            if (characterState.Animation_Data.IsRunning(typeof(WallSlide))) //prevent bugs when calculating several states at the short duration
            {
                return;
            }

            UpdateCharacterIgnoreTime(characterState.characterControl, stateInfo);

            if (characterState.characterControl.turbo)
            {
                animator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Turbo], true);
            }
            else
            {
                animator.SetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Turbo], false);
            }

            if (useMomentum)
            {
                MoveOnMomentum(characterState.characterControl, stateInfo);
            }
            else
            {
                if (constant)
                {
                    ConstantMove(characterState.characterControl, stateInfo);
                }
                else
                {
                    ControlledMove(characterState.characterControl, stateInfo);
                }
            }

            //Debug.Log($"<color=red>{IsBlocked(characterState.characterControl)}</color>");
        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (clearMomentumOnExit)
            {
                characterState.MomentumCalculator_Data.momentum = 0f;
            }
        }

        private void MoveOnMomentum(CharacterControl control, AnimatorStateInfo stateInfo)
        {
            var speed = speedGraph.Evaluate(stateInfo.normalizedTime) * Speed * Time.deltaTime;
            control.MOMENTUM_DATA.CalcualateMomentum(speed, maxMomentum);

            if (control.MOMENTUM_DATA.momentum > 0f)
            {
                control.ROTATION_DATA.FaceForward(true);
            }
            else if (control.MOMENTUM_DATA.momentum < 0f)
            {
                control.ROTATION_DATA.FaceForward(false);
            }

            if (!IsBlocked(control))
            {
                control.MoveForward(Speed, Mathf.Abs(control.MOMENTUM_DATA.momentum));
            }
        }

        private void ConstantMove(CharacterControl control, AnimatorStateInfo stateInfo)
        {
            if (!IsBlocked(control))
            {
                if (moveOnHit)
                {
                    if (!control.animationProgress.IsFacingAtacker())
                    {
                        control.MoveForward(Speed, speedGraph.Evaluate(stateInfo.normalizedTime));  //make sure speed is >0 in SO
                    }
                    else
                    {
                        control.MoveForward(-Speed, speedGraph.Evaluate(stateInfo.normalizedTime)); //make sure speed is >0 in SO
                    }
                }
                else
                {
                    control.MoveForward(Speed, speedGraph.Evaluate(stateInfo.normalizedTime));
                }
            }
        }

        private void ControlledMove(CharacterControl control, AnimatorStateInfo stateInfo)
        {
            if (control.moveRight && control.moveLeft)
            {
                return;
            }

            if (!control.moveRight && !control.moveLeft)
            {
                return;
            }

            if (control.moveRight)
            {
                if (!IsBlocked(control))
                {
                    control.MoveForward(Speed, speedGraph.Evaluate(stateInfo.normalizedTime));
                }
            }

            if (control.moveLeft)
            {
                if (!IsBlocked(control))
                {
                    control.MoveForward(Speed, speedGraph.Evaluate(stateInfo.normalizedTime));
                }
            }
            CheckTurn(control);
        }

        private void CheckTurn(CharacterControl control)
        {
            if (!lockDirection)
            {
                if (control.moveRight)
                {
                    control.ROTATION_DATA.FaceForward(true);
                }

                if (control.moveLeft)
                {
                    control.ROTATION_DATA.FaceForward(false);
                }
            }
        }

        private void UpdateCharacterIgnoreTime(CharacterControl control, AnimatorStateInfo stateInfo)
        {
            if (!ignoreCharacterBox)
            {
                control.animationProgress.isIgnoreCharacterTime = false;
            }

            if (stateInfo.normalizedTime > ignoreStartTime &&
                stateInfo.normalizedTime < ignoreEndTime)
            {
                control.animationProgress.isIgnoreCharacterTime = true;
            }
            else
            {
                control.animationProgress.isIgnoreCharacterTime = false;
            }
        }

        private bool IsBlocked(CharacterControl control)
        {
            if (control.BLOCKING_OBJ_DATA.frontBlockingDictionaryCount != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}