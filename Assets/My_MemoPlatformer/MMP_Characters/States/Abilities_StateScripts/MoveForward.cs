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

            if (allowEarlyTurn && !characterState.characterControl.animationProgress.disAllowEarlyTurn)
            {
                if (!characterState.characterControl.animationProgress.lockDirectionNextState)
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
                    characterState.characterControl.animationProgress.airMomentum = startingMomentum;
                }
                else
                {
                    characterState.characterControl.animationProgress.airMomentum = -startingMomentum;
                }
            }

            characterState.characterControl.animationProgress.disAllowEarlyTurn = false;
            characterState.characterControl.animationProgress.lockDirectionNextState = false;

            //if (moveOnHit)
            //{
                UpdateMoveOnHit(characterState.characterControl);

            //}
        }


        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (debug)
            {
                Debug.Log(stateInfo.normalizedTime);
            }

            characterState.characterControl.animationProgress.lockDirectionNextState = lockDirectionNextState;

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
                UpdateMomentum(characterState.characterControl, stateInfo);
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
                characterState.characterControl.animationProgress.airMomentum = 0f;
            }
        }

        private void UpdateMoveOnHit(CharacterControl control)
        {
            if (!moveOnHit)
            {
                return;
            }

            if (control.animationProgress.attacker != null)
            {
                Vector3 dir = control.transform.position - control.animationProgress.attacker.transform.position;

                if (dir.z > 0f)
                {
                    if (speed < 0f)
                    {
                        speed *= -1f;
                    }
                }
                else if (dir.z < 0f)
                {
                    if (speed > 0f)
                    {
                        speed *= -1f;
                    }
                }
            }
        }


        private void UpdateMomentum(CharacterControl control, AnimatorStateInfo stateInfo)
        {
            if (!control.animationProgress.RightSideIsBlocked())
            {
                if (control.moveRight)
                {
                    control.animationProgress.airMomentum += speedGraph.Evaluate(stateInfo.normalizedTime) * speed * Time.deltaTime;
                }
            }

            if (!control.animationProgress.LeftSideIsBlocked())
            {
                if (control.moveLeft)
                {
                    control.animationProgress.airMomentum -= speedGraph.Evaluate(stateInfo.normalizedTime) * speed * Time.deltaTime;
                }
            }

            if (control.animationProgress.RightSideIsBlocked() || control.animationProgress.LeftSideIsBlocked())
            {
                control.animationProgress.airMomentum = Mathf.Lerp(control.animationProgress.airMomentum, 0f, Time.deltaTime * 1.5f);
            }

            if (Mathf.Abs(control.animationProgress.airMomentum) >= maxMomentum)
            {
                if (control.animationProgress.airMomentum > 0f)
                {
                    control.animationProgress.airMomentum = maxMomentum;
                }
                else if (control.animationProgress.airMomentum < 0f)
                {
                    control.animationProgress.airMomentum = -maxMomentum;
                }
            }

            if (control.animationProgress.airMomentum > 0f)
            {
                control.FaceForward(true);
            }
            else if (control.animationProgress.airMomentum < 0f)
            {
                control.FaceForward(false);
            }

            if (!IsBlocked(control))
            {
                control.MoveForward(speed, Mathf.Abs(control.animationProgress.airMomentum));
            }

        }

        private void ConstantMove(CharacterControl control, Animator animator, AnimatorStateInfo stateInfo)
        {
            if (!IsBlocked(control))
            {
                control.MoveForward(speed, speedGraph.Evaluate(stateInfo.normalizedTime));
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
            if (control.animationProgress.blockingObjects.Count != 0)
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