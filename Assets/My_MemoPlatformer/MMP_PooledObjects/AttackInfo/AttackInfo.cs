using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{

    public class AttackInfo : MonoBehaviour
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

        public void ResetInfo(Attack attack, CharacterControl attacker)
        {
            isRegistered = false;
            isFinished = false;
            attackAbility = attack;
            this.attacker = attacker;
        }

        public void Register(Attack attack)
        {
            isRegistered = true;

            attackAbility = attack;
            //colliderNames = attack.colliderNames;
            attackParts = attack.attackParts;
            deathType = attack.deathType;
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