using UnityEngine;

namespace My_MemoPlatformer
{
    public class PlayerAttack : SubComponent
    {
        PlayerAttack_Data playerAttack_Data;
        private void Start()
        {
            playerAttack_Data = new PlayerAttack_Data
            {
                attackButtonIsReset = false,
                attackTriggered = false,
            };

            subComponentProcessor.subcomponentsDictionary.Add(SubComponentType.PLAYER_ATTACK, this);
            subComponentProcessor.playerAttack_Data = playerAttack_Data;

        }
        public override void OnFixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUpdate()
        {
            if (control.attack)
            { //dont trigger attack several times
                if (playerAttack_Data.attackButtonIsReset)
                {
                    playerAttack_Data.attackTriggered = true;
                    playerAttack_Data.attackButtonIsReset = false;
                }
            }
            else
            {
                playerAttack_Data.attackButtonIsReset = true;
                playerAttack_Data.attackTriggered = false;
            }
        }
    }
}