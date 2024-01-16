using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    [System.Serializable]
    public class Damage_Data
    {
        public AttackCondition blockedAttack;
        public float currentHp;
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
        [SerializeField] private CharacterControl _attacker = null;
        [SerializeField] private Attack _attack = null;
        [SerializeField] private GameObject _damagerPart = null;
        [SerializeField] private TriggerDetector _damaged_TG = null;
        [SerializeField] private Vector3 _incomingVelocity = Vector3.zero;

        public DamageTaken(
                CharacterControl attacker,
                Attack attack,
                TriggerDetector damaged_TG,
                GameObject damagerPart,
                Vector3 incomingVelocity)
        {
            _attacker = attacker;
            _attack = attack;
            _damaged_TG = damaged_TG;
            _damagerPart = damagerPart;
            _incomingVelocity = incomingVelocity;
        }

        public CharacterControl ATTACKER { get { return _attacker; } }
        public Attack ATTACK { get { return _attack; } }
        public GameObject DAMAGER { get { return _damagerPart; } }
        public TriggerDetector DAMAGE_TG { get { return _damaged_TG; } }
        public Vector3 INCOMING_VELOCITY { get { return _incomingVelocity; } }
    }
}
