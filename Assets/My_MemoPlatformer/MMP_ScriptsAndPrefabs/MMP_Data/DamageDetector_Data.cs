using UnityEngine;

namespace My_MemoPlatformer
{
    [System.Serializable]
    public class DamageDetector_Data
    {
        public CharacterControl attacker;
        public Attack attack;
        public TriggerDetector damagedTrigger;
        public GameObject attackingPart;
        public AttackCondition blockedAttack;
        public float hp;

        public Attack AxeThrow;
        public Attack airStompAttack;

        public delegate bool ReturnBool();
        public delegate void DoSomething(AttackCondition info);

        public ReturnBool IsDead;
        public DoSomething TakeDamage;

        public void SetData(CharacterControl attacker, Attack attack, TriggerDetector damagedTrigger, GameObject attackingPart)
        {
            this.attacker = attacker;
            this.attack = attack;
            this.damagedTrigger = damagedTrigger;
            this.attackingPart = attackingPart;
        }
    }
}

