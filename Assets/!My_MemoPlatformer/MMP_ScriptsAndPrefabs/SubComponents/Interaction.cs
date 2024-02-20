using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class Interaction : SubComponent
    {
        public Interaction_Data interaction_Data;
        private void Start()
        {
            interaction_Data = new Interaction_Data
            {
                GetTouchingWeapon = GetTouchingWeapon,
            };

            subComponentProcessor.arrSubComponents[(int)SubComponentType.INTERACTION] = this;
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
            foreach (KeyValuePair<TriggerDetector, List<Collider>> data in Control.DAMAGE_DATA.collidingWeapons_Dictionary)
            {
                var w = data.Value[0].gameObject.GetComponent<MeleeWeapon>();
                return w;
            }

            return null;
        }
    }
}