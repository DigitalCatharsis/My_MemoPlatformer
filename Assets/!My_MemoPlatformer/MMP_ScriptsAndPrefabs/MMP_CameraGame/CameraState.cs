using UnityEngine;
using Cinemachine;

namespace My_MemoPlatformer
{
    public class CameraState : StateMachineBehaviour
    {
        CharacterControl mainCharacter;

        CinemachineTransposer Transposer;
        float DefaultOffsetX;
        float ZoomOutOffsetX;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (Transposer == null)
            {
                Transposer = CameraManager.Instance.CAM_CONTROLLER.defaultCam.GetCinemachineComponent<CinemachineTransposer>();
                DefaultOffsetX = Transposer.m_FollowOffset.x;
                ZoomOutOffsetX = DefaultOffsetX * 10f;
            }

            for (int i = 0; i < (int)CameraTrigger.COUNT; i++)
            {
                CameraManager.Instance.CAM_CONTROLLER.ANIMATOR.ResetTrigger(HashManager.Instance.arrCameraParams[(int)i]);
            }
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (stateInfo.shortNameHash == HashManager.Instance.dicCameraStates[Camera_States.Shake])
            {
                if (stateInfo.normalizedTime > 0.7f)
                {
                    animator.SetTrigger(HashManager.Instance.arrCameraParams[(int)CameraTrigger.Default]);
                }
            }

            if (mainCharacter == null)
            {
                mainCharacter = CharacterManager.Instance.GetPlayableCharacter();
            }

            if (stateInfo.shortNameHash == HashManager.Instance.dicCameraStates[Camera_States.Default])
            {
                if (mainCharacter.skinnedMeshAnimator.GetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]))
                {
                    LerpNormal(CameraManager.Instance.CAM_CONTROLLER);
                }
                else
                {
                    if (mainCharacter.RIGID_BODY.velocity.y < 0f)
                    {
                        LerpZoomOut(CameraManager.Instance.CAM_CONTROLLER);
                    }
                }
            }
        }

        private void LerpZoomOut(CameraController camCon)
        {
            if (Transposer != null)
            {
                if (Mathf.Abs(Transposer.m_FollowOffset.x - ZoomOutOffsetX) > 0.1f)
                {
                    if (DebugContainer.Instance.debug_CameraState)
                    {
                        Debug.Log("lerping zoom out");
                    }
                    Transposer.m_FollowOffset.x = Mathf.Lerp(Transposer.m_FollowOffset.x, ZoomOutOffsetX, Time.deltaTime * camCon.zoomOutSpeed);
                }
            }
        }

        private void LerpNormal(CameraController camCon)
        {
            if (Transposer != null)
            {
                if (Mathf.Abs(Transposer.m_FollowOffset.x - DefaultOffsetX) > 0.1f)
                {
                    if (DebugContainer.Instance.debug_CameraState)
                    {
                        Debug.Log("lerping zoom int (back to default)");
                    }
                    Transposer.m_FollowOffset.x = Mathf.Lerp(Transposer.m_FollowOffset.x, DefaultOffsetX, Time.deltaTime * camCon.zoomInSpeed);
                }
            }
        }
    }
}