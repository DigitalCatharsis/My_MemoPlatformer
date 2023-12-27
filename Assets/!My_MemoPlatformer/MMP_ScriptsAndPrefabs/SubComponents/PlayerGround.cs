using UnityEngine;

namespace My_MemoPlatformer
{
    public class PlayerGround : SubComponent
    {
        public PlayerGround_Data playerGround_Data;
        private void Start()
        {
            playerGround_Data = new PlayerGround_Data
            {

            };

            control.subComponentProcessor.playerGround_Data = playerGround_Data;
            control.subComponentProcessor.subcomponentsDictionary.Add(SubComponentType.PLAYER_GROUND, this);
        }
        public override void OnFixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }

}
