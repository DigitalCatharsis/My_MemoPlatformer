using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class AttackCondition : MonoBehaviour
    {
        public CharacterControl attacker = null;
        public Attack attackAbility;
        //public List<string> colliderNames = new List<string>(); //name of the bodypards that gonna carry the attack
        public List<AttackPartType> attackParts = new List<AttackPartType>(); //name of the bodypards that gonna carry the attack
        public DeathType deathType;
        public bool mustCollide;
        public bool mustFaceAttacker;
        public float lethalRange;
        public int maxHits;
        public int currentHits;
        public bool isRegistered;
        public bool isFinished;
        public bool useRagdollDeath;
        public List<CharacterControl> registeredTargets = new List<CharacterControl>();  //using to prevent dublicate registering attacks

        public void ResetInfo(Attack attack, CharacterControl attacker)
        {
            isRegistered = false;
            isFinished = false;
            attackAbility = attack;
            this.attacker = attacker;
            registeredTargets.Clear();
        }

        public void Register(Attack attack)
        {
            isRegistered = true;

            attackAbility = attack;
            attackParts = attack.attackParts;
            mustCollide = attack.mustCollide;
            mustFaceAttacker = attack.mustFaceAttacker;
            lethalRange = attack.lethalRange;
            maxHits = attack.maxHits;
            currentHits = 0;
        }
        public void CopyInfo(Attack attack, CharacterControl attacker)
        {
            this.attacker = attacker;
            attackAbility = attack;
            attackParts = attack.attackParts;
            mustCollide = attack.mustCollide;
            mustFaceAttacker = attack.mustFaceAttacker;
            lethalRange = attack.lethalRange;
            maxHits = attack.maxHits;
            currentHits = 0;
        }

        private void OnDisable()
        {
            isFinished = true;
        }

    }
}