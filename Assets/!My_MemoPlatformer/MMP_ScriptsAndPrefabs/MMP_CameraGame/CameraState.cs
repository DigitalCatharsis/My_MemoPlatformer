using UnityEngine;
using Cinemachine;

namespace My_MemoPlatformer
{
    public class CameraState : StateMachineBehaviour
    {
        private CharacterControl _mainCharacter;
        private CinemachineTransposer _transposer;
        private float _defaultOffsetX;
        private float _zoomOutOffsetX;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_transposer == null)
            {
                _transposer = CameraManager.Instance.CAM_CONTROLLER.defaultCam.GetCinemachineComponent<CinemachineTransposer>();
                _defaultOffsetX = _transposer.m_FollowOffset.x;
                _zoomOutOffsetX = _defaultOffsetX * 10f;
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

            if (_mainCharacter == null)
            {
                _mainCharacter = CharacterManager.Instance.GetPlayableCharacter();
            }

            if (stateInfo.shortNameHash == HashManager.Instance.dicCameraStates[Camera_States.Default])
            {
                if (_mainCharacter.skinnedMeshAnimator.GetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]))
                {
                    LerpNormal(CameraManager.Instance.CAM_CONTROLLER);
                }
                else
                {
                    if (_mainCharacter.RIGID_BODY.velocity.y < 0f)
                    {
                        LerpZoomOut(CameraManager.Instance.CAM_CONTROLLER);
                    }
                }
            }
        }

        private void LerpZoomOut(CameraController camCon)
        {
            if (_transposer != null)
            {
                if (Mathf.Abs(_transposer.m_FollowOffset.x - _zoomOutOffsetX) > 0.1f)
                {
                    if (DebugContainer_Data.Instance.debug_CameraState)
                    {
                        Debug.Log("lerping zoom out");
                    }
                    _transposer.m_FollowOffset.x = Mathf.Lerp(_transposer.m_FollowOffset.x, _zoomOutOffsetX, Time.deltaTime * camCon.zoomOutSpeed);
                }
            }
        }

        private void LerpNormal(CameraController camCon)
        {
            if (_transposer != null)
            {
                if (Mathf.Abs(_transposer.m_FollowOffset.x - _defaultOffsetX) > 0.1f)
                {
                    if (DebugContainer_Data.Instance.debug_CameraState)
                    {
                        Debug.Log("lerping zoom int (back to default)");
                    }
                    _transposer.m_FollowOffset.x = Mathf.Lerp(_transposer.m_FollowOffset.x, _defaultOffsetX, Time.deltaTime * camCon.zoomInSpeed);
                }
            }
        }
    }
}