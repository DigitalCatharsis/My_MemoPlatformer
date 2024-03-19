using System;
using System.Collections;
using UnityEngine;

namespace My_MemoPlatformer
{
    [Serializable]
    public class Interaction_Data
    {
        public Func<MeleeWeapon> GetTouchingWeapon;
        public Action<CharacterControl, VFXType, string, bool, float> SpawnParticle;
        public Action<CharacterControl, VFXType> SpawnPointHitParticle;
    }
}