using System.Collections.Generic;
using UnityEngine;
namespace My_MemoPlatformer
{
    public class PlayerJump : SubComponent
    {
        [SerializeField] private Jump_Data _jump_Data;

        private void OnEnable()
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
            throw new System.NotImplementedException();
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }

}

