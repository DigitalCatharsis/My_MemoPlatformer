using System.Collections;
using System.Linq;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class VerticalVelocity : SubComponent
    {
        public VerticalVelocity_Data verticalVelocity_Data;

        public override void OnComponentEnabled()
        {
            verticalVelocity_Data = new VerticalVelocity_Data
            {
                noJumpCancel = false,
                maxWallSlideVelocity = Vector3.zero,
            };
            subComponentProcessor.verticalVelocity_Data = verticalVelocity_Data;
        }

        public override void OnFixedUpdate()
        {
            // jump cancel after letting go JumpKey
            if (!verticalVelocity_Data.noJumpCancel)
            {
                if (control.rigidBody.velocity.y > 0f && !control.jump)
                {
                    control.rigidBody.velocity -= (Vector3.up * control.rigidBody.velocity.y * 0.1f);    //Высота прыжка в зависимости от длительности нажатия
                }
            }

            //slow down wallslide
            if (verticalVelocity_Data.maxWallSlideVelocity.y != 0f)
            {
                if (control.rigidBody.velocity.y <= verticalVelocity_Data.maxWallSlideVelocity.y)
                {
                    control.rigidBody.velocity = verticalVelocity_Data.maxWallSlideVelocity;
                }
            }
        }

        public override void OnUpdate()
        {
        }
    }
}
