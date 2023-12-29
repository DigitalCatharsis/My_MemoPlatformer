using System;

namespace My_MemoPlatformer
{
    [Serializable]
    public class MomentumCalculator_Data
    {
        public float momentum;
        public delegate void DoSomething(float speed, float maxMomentum);
        public DoSomething CalcualateMomentum;
    }
}
