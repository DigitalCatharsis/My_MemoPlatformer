using UnityEngine;

namespace My_MemoPlatformer
{
    public class PlayerRotation : SubComponent
    {
        PlayerRotation_Data playerRotation_Data;
        static string L_CharacterSelect = "L_CharacterSelect";
        private void Start()
        {
            playerRotation_Data = new PlayerRotation_Data
            {
                lockDirectionNextState = false,
                lockEarlyTurn = false,
                EarlyTurnIsLocked = EarlyTurnIsLocked,
                FaceForward = FaceForward,
                IsFacingForward = IsFacingForward,
            };

            subComponentProcessor.playerRotation_Data = playerRotation_Data;
        }
        public override void OnFixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }

        private bool EarlyTurnIsLocked()
        {
            if (playerRotation_Data.lockEarlyTurn || playerRotation_Data.lockDirectionNextState)
            {
                return true;
            }
            else
            {
                return false;
            }
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
    }
}
