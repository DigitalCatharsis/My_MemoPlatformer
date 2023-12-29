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

            control.subComponentProcessor.ground_Data = playerGround_Data;
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
