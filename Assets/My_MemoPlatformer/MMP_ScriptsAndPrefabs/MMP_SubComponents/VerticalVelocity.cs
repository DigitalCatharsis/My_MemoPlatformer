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
            subComponentProcessor.componentsDictionary.Add(SubComponentType.VERTICALVELOCITY_DATA, this);
        }

        public override void OnFixedUpdate()
        {
            // jump cancel after letting go JumpKey
            if (!verticalVelocity_Data.noJumpCancel)
            {
                if (control.Rigid_Body.velocity.y > 0f && !control.jump)
                {
                    control.Rigid_Body.velocity -= (Vector3.up * control.Rigid_Body.velocity.y * 0.1f);    //Высота прыжка в зависимости от длительности нажатия
                }
            }

            //slow down wallslide
            if (verticalVelocity_Data.maxWallSlideVelocity.y != 0f)
            {
                if (control.Rigid_Body.velocity.y <= verticalVelocity_Data.maxWallSlideVelocity.y)
                {
                    control.Rigid_Body.velocity = verticalVelocity_Data.maxWallSlideVelocity;
                }
            }
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }
    }
}
