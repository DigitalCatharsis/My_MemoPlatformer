using UnityEngine;

namespace My_MemoPlatformer
{
    public class PlayerRotation : SubComponent
    {
        public Rotation_Data rotation_Data;
        //TODO: remove
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

            if (!Control.skinnedMeshAnimator.enabled)   //to prevent rotating after death
            {
                return;
            }

            if (Control.ROTATION_DATA.lockTurn)
            {
                return;
            }

            if (forward)
            {
                Control.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

            }
            else
            {
                Control.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
        private bool IsFacingForward()
        {
            if (Control.transform.forward.z > 0f)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void ClearTurnLock()
        {
            if (!Control.PLAYER_ANIMATION_DATA.IsRunning(typeof(LockTurn)))
            {
                if (rotation_Data.lockTurn)
                {
                    AnimatorStateInfo info = Control.skinnedMeshAnimator.GetCurrentAnimatorStateInfo(0);

                    if (info.normalizedTime >= rotation_Data.unlockTiming)
                    {
                        rotation_Data.lockTurn = false;
                    }
                }
            }
        }
    }
}
