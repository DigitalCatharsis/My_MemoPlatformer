using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [System.Serializable]
    public class Damage_Data
    {
        public AttackCondition blockedAttack;
        public float hp;
        public Attack airStompAttack;
        public Attack weaponThrow;

        public DamageTaken damageTaken;

        [Header("Colliding Objects")]
        public Dictionary<TriggerDetector, List<Collider>> collidingBodyParts_Dictionary = new Dictionary<TriggerDetector, List<Collider>>(); //key trigger detectors, value - colliding bodyparts which are in contract with trigger detectors
        public Dictionary<TriggerDetector, List<Collider>> collidingWeapons_Dictionary = new Dictionary<TriggerDetector, List<Collider>>(); //key trigger detectors, value - colliding bodyparts which are in contract with trigger detectors

        public delegate bool ReturnBool();
        public delegate void DoDamage(AttackCondition info);
        public delegate void DoCollateralDamage(CharacterControl attacker, Collider col, TriggerDetector triggerDetector);
        public delegate void EditColliderDictionary(Dictionary<TriggerDetector, List<Collider>> colliders_Dictionary, Collider collider, TriggerDetector triggerDetector);

        public ReturnBool IsDead;
        public DoDamage TakeDamage;
        public DoCollateralDamage TakeCollateralDamage;
        public EditColliderDictionary AddCollidersToDictionary;
        public EditColliderDictionary RemoveCollidersFromDictionary;
    }

    [System.Serializable]
    public class DamageTaken
    {
        public DamageTaken(
                CharacterControl attacker,
                Attack attack,
                TriggerDetector damaged_TG,
                GameObject damager,
                Vector3 incomingVelocity)
        {
            mAttacker = attacker;
            mAttack = attack;
            mDamage_TG = damaged_TG;
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
