using UnityEngine;

namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "New state", menuName = "My_MemoPlatformer/AbilityData/OffsetOnLedge")]
    public class OffsetOnLedge : StateData
    {
        public override void OnEnter(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
            GameObject anim = characterState.characterControl.skinnedMeshAnimator.gameObject;
            anim.transform.parent = characterState.characterControl.ledgeChecker.grabbedLedge.transform;
            anim.transform.localPosition = characterState.characterControl.ledgeChecker.grabbedLedge.offset;

            #region ledgeGrabCalibration  //untested
            float x;
            float y;
            float z;

            if (characterState.characterControl.IsFacingForward())
            {
                x = characterState.characterControl.ledgeChecker.ledgeCalibration.x;
                y = characterState.characterControl.ledgeChecker.ledgeCalibration.y;
                z = characterState.characterControl.ledgeChecker.ledgeCalibration.z;
            }
            else
            {
                x = characterState.characterControl.ledgeChecker.ledgeCalibration.x;
                y = characterState.characterControl.ledgeChecker.ledgeCalibration.y;
                z = -characterState.characterControl.ledgeChecker.ledgeCalibration.z;
            }

            Vector3 calibration;
            calibration.x = x;
            calibration.z = z;
            calibration.y = y;

            anim.transform.localPosition += calibration;
            #endregion

            characterState.characterControl.Rigid_Body.velocity= Vector3.zero;
        }

        public override void UpdateAbility(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {

        }

        public override void OnExit(CharacterState characterState, Animator animator, AnimatorStateInfo stateInfo)
        {
            
        }
    }

}
