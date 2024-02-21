using UnityEngine;

namespace My_MemoPlatformer
{
    public class PlayerGround : SubComponent
    {
        public Ground_Data playerGround_Data;
        private void Start()
        {
            playerGround_Data = new Ground_Data
            {

            };

            Control.subComponentProcessor.ground_Data = playerGround_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.PLAYER_GROUND] = this;
        }
        public override void OnFixedUpdate()
        {
        }

        public override void OnUpdate()
        {
        }
    }

}
