using System;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    public class Attack_Data
    {
        public bool attackTriggered;
        public bool attackButtonIsReset;

        [Header("AirStomp")]
        public Attack airStompAttack;

        [Header("Weapon")]
        public MeleeWeapon holdingWeapon;
        public Attack weaponThrowAttack;
    }
}
