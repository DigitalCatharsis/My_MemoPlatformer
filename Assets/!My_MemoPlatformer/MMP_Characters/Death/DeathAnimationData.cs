using UnityEngine;

namespace My_MemoPlatformer
{
    public enum DeathType
    {
        NONE,
        LAUNCH_INTO_AIR,
        GROUND_SHOCK,
    }

    [CreateAssetMenu(fileName ="New ScriptableObject", menuName = "My_MemoPlatformer/Death/DeathAnimationData")]
    public class DeathAnimationData : ScriptableObject
    {
        public RuntimeAnimatorController animator;
        public DeathType deathType;
        public bool isFacingAttacker;
    }
}
