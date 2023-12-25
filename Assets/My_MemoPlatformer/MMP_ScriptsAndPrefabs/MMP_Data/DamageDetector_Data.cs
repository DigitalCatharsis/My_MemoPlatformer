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
        public AttackInfo blockedAttack;

        public delegate bool ReturnBool();

        public ReturnBool IsDead;

        public void SetData(CharacterControl attacker, Attack attack, TriggerDetector damagedTrigger, GameObject attackingPart)
        {
            this.attacker = attacker;
            this.attack = attack;
            this.damagedTrigger = damagedTrigger;
            this.attackingPart = attackingPart;
        }
    }
}

