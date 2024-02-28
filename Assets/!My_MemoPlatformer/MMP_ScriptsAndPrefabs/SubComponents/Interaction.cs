using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class Interaction : SubComponent
    {
        public Interaction_Data interaction_Data;
        public override void OnComponentEnabled()
        {
            interaction_Data = new Interaction_Data
            {
                GetTouchingWeapon = GetTouchingWeapon,
            };

            subComponentProcessor.interaction_Data = interaction_Data;
        }

        public override void OnFixedUpdate()
        {

        }

        public override void OnUpdate()
        {

        }
        public MeleeWeapon GetTouchingWeapon()
        {
            foreach (KeyValuePair<TriggerDetector, List<Collider>> data in control.DAMAGE_DATA.collidingWeapons_Dictionary)
            {
                var w = data.Value[0].gameObject.GetComponent<MeleeWeapon>();
                return w;
            }

            return null;
        }
    }
}