using UnityEngine;

namespace My_MemoPlatformer
{
    public class VerticalVelocity : SubComponent
    {
        public VerticalVelocity_Data verticalVelocity_Data;

        private void Start()
        {
            verticalVelocity_Data = new VerticalVelocity_Data
            {
                noJumpCancel = false,
                maxWallSlideVelocity = Vector3.zero,
            };
            subComponentProcessor.verticalVelocity_Data = verticalVelocity_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.VERTICAL_VELOCITY] = this;
        }

        public override void OnFixedUpdate()
        {
            // jump cancel after letting go JumpKey
            if (!verticalVelocity_Data.noJumpCancel)
            {
                if (Control.rigidBody.velocity.y > 0f && !Control.jump)
                {
                    Control.rigidBody.velocity -= (Vector3.up * Control.rigidBody.velocity.y * 0.1f);    //Высота прыжка в зависимости от длительности нажатия
                }
            }

            //slow down wallslide
            if (verticalVelocity_Data.maxWallSlideVelocity.y != 0f)
            {
                if (Control.rigidBody.velocity.y <= verticalVelocity_Data.maxWallSlideVelocity.y)
                {
                    Control.rigidBody.velocity = verticalVelocity_Data.maxWallSlideVelocity;
                }
            }
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}
