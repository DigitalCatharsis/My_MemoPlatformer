
using System;
using UnityEngine;
namespace My_MemoPlatformer
{
    [Serializable]
    public class Interaction_Data
    {
        public Func<MeleeWeapon> GetTouchingWeapon;
    }
}