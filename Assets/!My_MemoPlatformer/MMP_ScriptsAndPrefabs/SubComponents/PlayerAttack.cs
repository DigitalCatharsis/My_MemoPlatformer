using System.Collections;
using System.Linq;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class PlayerAttack : SubComponent
    {
        public Attack_Data playerAttack_Data;

        [Header("Damage Setup")]
        [SerializeField] private Attack _weaponThrow;
        [SerializeField] private Attack _airStompAttack;

        public override void OnComponentEnabled()
        {
            playerAttack_Data = new Attack_Data
            {
                holdingWeapon = null,
                attackButtonIsReset = false,
                attackTriggered = false,

                airStompAttack = _airStompAttack,
                weaponThrowAttack = _weaponThrow,
            };

            subComponentProcessor.attack_Data = playerAttack_Data;
        }

        public override void OnFixedUpdate()
        {
        }

        public override void OnUpdate()
        {
            if (control.attack)
            { //dont trigger attack several times
                if (playerAttack_Data.attackButtonIsReset)
                {
                    playerAttack_Data.attackTriggered = true;
                    playerAttack_Data.attackButtonIsReset = false;
                }
            }
            else
            {
                playerAttack_Data.attackButtonIsReset = true;
                playerAttack_Data.attackTriggered = false;
            }
        }
    }
}
