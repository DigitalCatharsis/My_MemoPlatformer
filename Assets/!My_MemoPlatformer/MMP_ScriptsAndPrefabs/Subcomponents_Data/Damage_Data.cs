using UnityEngine;

namespace My_MemoPlatformer
{
    [System.Serializable]
    public class Damage_Data
    {
        public AttackCondition blockedAttack;
        public float hp;
        public Attack airStompAttack;
        public Attack AxeThrow;

        public DamageTaken damageTaken;

        public delegate bool ReturnBool();
        public delegate void DoSomething(AttackCondition info);

        public ReturnBool IsDead;
        public DoSomething TakeDamage;
    }

    [System.Serializable]
    public class DamageTaken
    {
        public DamageTaken(
                CharacterControl attacker,
                Attack attack,
                TriggerDetector damage_TG,
                GameObject damager,
                Vector3 incomingVelocity)
        {
            mAttacker = attacker;
            mAttack = attack;
            mDamage_TG = damage_TG;
            mDamager = damager;
            mIncomingVelocity = incomingVelocity;
        }

        [SerializeField] CharacterControl mAttacker = null;
        [SerializeField] Attack mAttack = null;
        [SerializeField] GameObject mDamager = null;
        [SerializeField] TriggerDetector mDamage_TG = null;
        [SerializeField] Vector3 mIncomingVelocity = Vector3.zero;

        public CharacterControl ATTACKER { get { return mAttacker; } }
        public Attack ATTACK { get { return mAttack; } }
        public GameObject DAMAGER { get { return mDamager; } }
        public TriggerDetector DAMAGE_TG { get { return mDamage_TG; } }
        public Vector3 INCOMING_VELOCITY { get { return mIncomingVelocity; } }
    }
}
