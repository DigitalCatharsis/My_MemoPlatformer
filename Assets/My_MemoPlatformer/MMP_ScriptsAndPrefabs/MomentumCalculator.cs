using UnityEngine;

namespace My_MemoPlatformer
{
    public class MomentumCalculator : SubComponent
    {
        public MomentumCalculator_Data momentumCalculator_Data;

        private void Start()
        {
            momentumCalculator_Data = new MomentumCalculator_Data
            {
                momentum = 0,
                CalcualteMomentum = CalculateMomentum,
            };

            subComponentProcessor.momentumCalculator_Data = momentumCalculator_Data;
        }

        public override void OnFixedUpdate()
        {
            momentumCalculator_Data = subComponentProcessor.momentumCalculator_Data;
        }

        public override void OnUpdate()
        {

        }

        private void CalculateMomentum(float speed, float maxMomentum)
        {
            if (!control.BlockingObj_Data.RightSideBLocked())
            {
                if (control.moveRight)
                {
                    momentumCalculator_Data.momentum += speed;
                }
            }

            if (!control.BlockingObj_Data.LeftSideBlocked())
            {
                if (control.moveLeft)
                {
                    momentumCalculator_Data.momentum -= speed;
                }
            }

            if (control.BlockingObj_Data.RightSideBLocked() || control.BlockingObj_Data.LeftSideBlocked())
            {
                var lerped = Mathf.Lerp(momentumCalculator_Data.momentum, 0f, Time.deltaTime * 1.5f);
                momentumCalculator_Data.momentum = lerped;
            }

            if (Mathf.Abs(momentumCalculator_Data.momentum) >= maxMomentum)
            {
                if (momentumCalculator_Data.momentum > 0f)
                {
                    momentumCalculator_Data.momentum = maxMomentum;
                }
                else if (momentumCalculator_Data.momentum < 0f)
                {
                    momentumCalculator_Data.momentum = -maxMomentum;
                }
            }
        }
    }
}
