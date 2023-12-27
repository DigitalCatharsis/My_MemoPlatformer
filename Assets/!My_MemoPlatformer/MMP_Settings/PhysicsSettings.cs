using UnityEngine;


namespace My_MemoPlatformer
{
    [CreateAssetMenu(fileName = "Settings", menuName = "My_MemoPlatformer/Settings/PhysicsSettings")]

    public class PhysicsSettings : ScriptableObject
    {
        public int defaultSolverIterations;
        public int defaultSolverVelocityIterations;

    }
}
