using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class TriggerDetector : MonoBehaviour
    {
        public CharacterControl control;
        public Collider triggerCollider;
        public Rigidbody rigidBody;

        public Vector3 lastPosition;
        public Quaternion lastRotation;

        private void Awake()
        {
            control = this.GetComponentInParent<CharacterControl>();
            triggerCollider = this.gameObject.GetComponent<Collider>();
            rigidBody = this.gameObject.GetComponent<Rigidbody>();
        }


        private void OnTriggerEnter(Collider collider) //callback function whenever somth touches or enters raggdoll body parts
        {
            var attacker = GetCollidingCharacter(collider);

            if (attacker != null)
            {
                control.DAMAGE_DATA.TakeCollateralDamage(attacker, collider, this);
            }

            var weapon = CheckForCollidingWeapon(collider);

            if (weapon != null)
            {
                if (weapon.isThrown)
                {
                    if (weapon.thrower != control)
                    {
                        GetHitByThrowedWeapon(weapon);
                        RepositionThrowedWeaponAtHit(weapon);
                    }
                }
                control.DAMAGE_DATA.AddCollidersToDictionary(control.DAMAGE_DATA.collidingWeapons_Dictionary, collider, this);
            }
        }

        private CharacterControl GetCollidingCharacter(Collider hittedCollider)
        {
            if (control == null)
            {
                return null;
            }

            for (int i = 0; i < control.RAGDOLL_DATA.arrBodyParts.Length; i++) //not self
            {
                if (control.RAGDOLL_DATA.arrBodyParts[i].Equals(hittedCollider))
                {
                    return null;
                }
            }

            var attacker = CharacterManager.Instance.GetCharacter(hittedCollider.transform.root.gameObject);

            if (attacker == null)
            {
                return null;
            }

            if (hittedCollider.gameObject == attacker.gameObject) //not a boxCollider?
            {
                return null;
            }

            control.DAMAGE_DATA.AddCollidersToDictionary(control.DAMAGE_DATA.collidingBodyParts_Dictionary, hittedCollider, this);

            return attacker;
        }

        private MeleeWeapon CheckForCollidingWeapon(Collider collider)
        {
            var weapon = collider.transform.root.gameObject.GetComponent<MeleeWeapon>();

            if (weapon == null)
            {
                return null;
            }

            return weapon;
        }

        private void GetHitByThrowedWeapon(MeleeWeapon weapon)
        {
            var attackCondition_info = new AttackCondition();
            attackCondition_info.CopyInfo(control.DAMAGE_DATA.weaponThrow, control);

            control.DAMAGE_DATA.damageTaken = new DamageTaken(
                attacker: weapon.thrower,
                attack: control.DAMAGE_DATA.weaponThrow,
                damaged_TG: this,
                damagerPart: null,
                incomingVelocity: Vector3.zero);

            control.DAMAGE_DATA.TakeDamage(attackCondition_info);

            if (weapon.flyForward)
            {
                //weapon.transform.rotation = Quaternion.Euler(0f, 90f, 45f);
            }
            else
            {
                //weapon.transform.rotation = Quaternion.Euler(0f, -90f, 45f);
            }
            weapon.isThrown = false;
            return;
        }

        private void RepositionThrowedWeaponAtHit(MeleeWeapon weapon)
        {
            var damagedPart = this.transform.gameObject;
            if (damagedPart.name.Contains("Hand") || damagedPart.name.Contains("Arm"))
            {
                while (!damagedPart.name.Contains("Spine1"))
                {
                    if (DebugContainer_Data.Instance.debug_TriggerDetector)
                    {
                        Debug.Log($"Changing parent from {damagedPart} to {damagedPart.transform.parent.name}");
                    }
                    damagedPart = damagedPart.transform.parent.gameObject;
                }
            }
            weapon.transform.parent = damagedPart.transform;

            var offset = damagedPart.transform.position - weapon.weaponTip.transform.position;
            weapon.transform.position += offset;
        }

        private void OnTriggerExit(Collider col)
        {
            CheckExitingBodypart(col);
            CheckExitingWeapons(col);
        }

        private void CheckExitingBodypart(Collider collider)
        {
            if (control == null)
            {
                return;
            }

            if (control.DAMAGE_DATA.collidingBodyParts_Dictionary.ContainsKey(this))
            {
                control.DAMAGE_DATA.RemoveCollidersFromDictionary(control.DAMAGE_DATA.collidingBodyParts_Dictionary, collider, this);
            }
        }

        private void CheckExitingWeapons(Collider collider)
        {
            if (control == null)
            {
                return;
            }

            if (control.DAMAGE_DATA.collidingWeapons_Dictionary.ContainsKey(this))
            {
                control.DAMAGE_DATA.RemoveCollidersFromDictionary(control.DAMAGE_DATA.collidingWeapons_Dictionary, collider, this);
            }
        }
    }
}
