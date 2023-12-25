using UnityEngine;

namespace My_MemoPlatformer
{
    public class PlayerRotation : SubComponent
    {
        PlayerRotation_Data playerRotation_Data;

        private void Start()
        {
            playerRotation_Data = new PlayerRotation_Data
            {
                lockDirectionNextState = false,
                lockEarlyTurn = false,
                EarlyTurnIsLocked = EarlyTurnIsLocked,
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
    }
}
