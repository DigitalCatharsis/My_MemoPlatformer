using UnityEngine;

namespace My_MemoPlatformer
{
    public class VerticalVelocity : SubComponent
    {
        private VerticalVelocity_Data verticalVelocity_Data;

        private void Start()
        {
            verticalVelocity_Data = new VerticalVelocity_Data
            {
                noJumpCancel = false,
                maxWallSlideVelocity = Vector3.zero,
            };
            subComponentProcessor.verticalVelocity_Data = verticalVelocity_Data;
            subComponentProcessor.subcomponentsDictionary.Add(SubComponentType.VERTICALVELOCITY_DATA, this);
        }

        public override void OnFixedUpdate()
        {
            // jump cancel after letting go JumpKey
            if (!verticalVelocity_Data.noJumpCancel)
            {
                if (control.RIGID_BODY.velocity.y > 0f && !control.jump)
                {
                    control.RIGID_BODY.velocity -= (Vector3.up * control.RIGID_BODY.velocity.y * 0.1f);    //Высота прыжка в зависимости от длительности нажатия
                }
            }

            //slow down wallslide
            if (verticalVelocity_Data.maxWallSlideVelocity.y != 0f)
            {
                if (control.RIGID_BODY.velocity.y <= verticalVelocity_Data.maxWallSlideVelocity.y)
                {
                    control.RIGID_BODY.velocity = verticalVelocity_Data.maxWallSlideVelocity;
                }
            }
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}
