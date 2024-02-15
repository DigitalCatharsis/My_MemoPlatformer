using System;

namespace My_MemoPlatformer
{
    [Serializable]
    public class MomentumCalculator_Data
    {
        public float momentum;

        public Action<float,float> CalcualateMomentum;
    }
}
