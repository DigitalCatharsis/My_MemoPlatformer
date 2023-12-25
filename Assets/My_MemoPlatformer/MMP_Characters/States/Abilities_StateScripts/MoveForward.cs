using My_MemoPlatformer.Datasets;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;

namespace My_MemoPlatformer
{

    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/MoveForward")]
    public class MoveForward : StateData
    {
        [SerializeField] private bool debug;

        [Tooltip("Prevent turning when running from idle")] public bool allowEarlyTurn;
        public bool lockDirection;
        public bool lockDirectionNextState;
        [Tooltip("Move no matter what")] public bool constant;
        public AnimationCurve speedGraph;
        public float speed;
        [Tooltip("Distance to prevent moving, \n MAKE SURE ITS LESS THAN ColliderEdge RADIUS!")] public float blockDistance;

        [Header("IgnoreCharacterBox")]
        public bool ignoreCharacterBox;
        public float ignoreStartTime;
        public float ignoreEndTime;

        [Header("Momentum")]
        public bool useMomentum;
        public float startingMomentum;
        public float maxMomentum;
        public bool clearMomentumOnExit;

        [Header("MoveOnHit")]
        [Tooltip("Move to direction away from Attacker")] public bool moveOnHit;

        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            characterState.characterControl.animationProgress.latestMoveForwardScript = this;

            if (allowEarlyTurn)
            {
                // early turn can be locked by previous states
                if (!characterState.PlayerRotation_Data.EarlyTurnIsLocked())
                {
                    if (characterState.characterControl.moveLeft)
                    {
                        characterState.characterControl.FaceForward(false);
                    }
                    if (characterState.characterControl.moveRight)
                    {
                        characterState.characterControl.FaceForward(true);
                    }
                }
            }

            if (startingMomentum > 0.001f)
            {
                if (characterState.characterControl.IsFacingForward())
                {
                    characterState.MomentumCalculator_Data.momentum = startingMomentum;
                }
                else
                {
                    characterState.MomentumCalculator_Data.momentum = - startingMomentum;
                }
            }

            characterState.PlayerRotation_Data.lockEarlyTurn = false;
            characterState.PlayerRotation_Data.lockDirectionNextState = false;


        }


        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (debug)
            {
                Debug.Log(stateInfo.normalizedTime);
            }

            characterState.PlayerRotation_Data.lockDirectionNextState = lockDirectionNextState;

            if (characterState.characterControl.animationProgress.latestMoveForwardScript != this)
            {
                return;
            }

            if (characterState.characterControl.animationProgress.IsRunning(typeof(WallSlide))) //prevent bugs when calculating several states at the short duration
            {
                return;
            }

            UpdateCharacterIgnoreTime(characterState.characterControl, stateInfo);

            if (characterState.characterControl.jump)
            {
                if (characterState.characterControl.animationProgress.ground != null)
                {
                    animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Jump], true);
                }
            }

            if (characterState.characterControl.turbo)
            {
                animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Turbo], true);
            }
            else
            {
                animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Turbo], false);
            }

            if (useMomentum)
            {
                MoveOnMomentum(characterState.characterControl, stateInfo);
            }
            else
            {
                if (constant)
                {
                    ConstantMove(characterState.characterControl, animator, stateInfo);
                }
                else
                {
                    ControlledMove(characterState.characterControl, animator, stateInfo);
                }
            }
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
            var currentSpeed = speedGraph.Evaluate(stateInfo.normalizedTime) * speed * Time.deltaTime;
            control.MomentumCalculator_Data.CalcualteMomentum(currentSpeed, maxMomentum);

            if (control.MomentumCalculator_Data.momentum > 0f)
            {
                control.FaceForward(true);
            }
            else if (control.MomentumCalculator_Data.momentum < 0f)
            {
                control.FaceForward(false);
            }

            if (!IsBlocked(control))
            {
                control.MoveForward(speed, Mathf.Abs(control.MomentumCalculator_Data.momentum));
            }

        }

        private void ConstantMove(CharacterControl control, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!IsBlocked(control))
            {
                if (moveOnHit)
                {
                    if (!control.animationProgress.IsFacingAtacker())
                    {
                        control.MoveForward(speed, speedGraph.Evaluate(stateInfo.normalizedTime));  //make sure speed is >0 in SO
                    }
                    else
                    {
                        control.MoveForward(-speed, speedGraph.Evaluate(stateInfo.normalizedTime)); //make sure speed is >0 in SO
                    }
                }
                else
                {
                    control.MoveForward(speed, speedGraph.Evaluate(stateInfo.normalizedTime));
                }               
            }

            if (!control.moveRight && !control.moveLeft)
            {
                animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Move], false);
            }
            else
            {
                animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Move], true);
            }
        }

        private void ControlledMove(CharacterControl control, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (control.moveRight && control.moveLeft)
            {
                animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Move], false);
                return;
            }

            if (!control.moveRight && !control.moveLeft)
            {
                animator.SetBool(HashManager.Instance.dicMainParams[TransitionParameter.Move], false);
                return;
            }

            if (control.moveRight)
            {
                if (!IsBlocked(control))
                {
                    control.MoveForward(speed, speedGraph.Evaluate(stateInfo.normalizedTime));
                }
            }

            if (control.moveLeft)
            {
                {
                    if (!IsBlocked(control))
                    {
                        control.MoveForward(speed, speedGraph.Evaluate(stateInfo.normalizedTime));
                    }
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
                    control.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                }
                if (control.moveLeft)
                {
                    control.transform.rotation = Quaternion.Euler(0f, 180, 0f);
                }
            }
        }

        private void UpdateCharacterIgnoreTime(CharacterControl control, AnimatorStateInfo stateInfo)
        {
            if (!ignoreCharacterBox)
            {
                control.animationProgress.isIgnoreCharacterTime = false;
            }

            if (stateInfo.normalizedTime > ignoreStartTime
                && stateInfo.normalizedTime < ignoreEndTime)
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
            if (control.BlockingObj_Data.frontBlockingDictionaryCount != 0)
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