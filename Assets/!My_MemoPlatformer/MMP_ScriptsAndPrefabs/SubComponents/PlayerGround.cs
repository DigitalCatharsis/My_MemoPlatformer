using System.Collections;
using System.Linq;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class PlayerGround : SubComponent
    {
        public Ground_Data playerGround_Data;
        public override void OnComponentEnabled()
        {
            playerGround_Data = new Ground_Data
            {

            };
                        
            subComponentProcessor.ground_Data = playerGround_Data;
        }
        public override void OnFixedUpdate()
        {
        }

        public override void OnUpdate()
        {
        }
    }

}
