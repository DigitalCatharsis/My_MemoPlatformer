using My_MemoPlatformer.Roundbeargames;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class PlayerRotation : SubComponent
    {
        Rotation_Data rotation_Data;
        static string L_CharacterSelect = "L_CharacterSelect";
        private void Start()
        {
            rotation_Data = new Rotation_Data
            {
                lockTurn = false,
                unlockTiming = 0f,
                FaceForward = FaceForward,
                IsFacingForward = IsFacingForward,
            };

            subComponentProcessor.rotation_Data = rotation_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.PLAYER_ROTATION] = this;
        }
        public override void OnFixedUpdate()
        {
            ClearTurnLock();
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
        private void FaceForward(bool forward)
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals(L_CharacterSelect))
            {
                return;
            }

            if (!control.skinnedMeshAnimator.enabled)   //to prevent rotating after death
            {
                return;
            }

            if (control.ROTATION_DATA.lockTurn)
            {
                return;
            }

            if (forward)
            {
                control.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            }
            else
            {
                control.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
        private bool IsFacingForward()
        {
            if (control.transform.forward.z > 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        void ClearTurnLock()
        {
            if (!control.ANIMATION_DATA.IsRunning(typeof(LockTurn)))
            {
                if (rotation_Data.lockTurn)
                {
                    AnimatorStateInfo info = control.skinnedMeshAnimator.GetCurrentAnimatorStateInfo(0);

                    if (info.normalizedTime >= rotation_Data.unlockTiming)
                    {
                        rotation_Data.lockTurn = false;
                    }
                }
            }
        }
    }
}
