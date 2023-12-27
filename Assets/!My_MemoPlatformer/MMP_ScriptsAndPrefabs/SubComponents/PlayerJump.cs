using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace My_MemoPlatformer
{
    public class PlayerJump : SubComponent
    {
        PlayerJump_Data PlayerJump_Data;

        private void Start()
        {
            PlayerJump_Data = new PlayerJump_Data
            {
                jumped = false,
                canWallJump = false,
                checkWallBlock = false,
            };

            subComponentProcessor.playerJump_Data = PlayerJump_Data;
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

