using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace My_MemoPlatformer
{
    public class PlayerJump : SubComponent
    {
        [SerializeField] private Jump_Data _jump_Data;

        public override void OnComponentEnabled()
        {
            _jump_Data = new Jump_Data
            {
                dicJumped = new Dictionary<int, bool>(),
                canWallJump = false,
                checkWallBlock = false,
            };

            subComponentProcessor.jump_Data = _jump_Data;
        }
        public override void OnFixedUpdate()
        {
        }

        public override void OnUpdate()
        {
        }
    }

}

