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


        private void OnTriggerEnter(Collider col) //callback function whenever somth touches or enters raggdoll body parts
        {
            var attacker = CheckCollidingBodyparts(col);

            if (attacker != null)
            {
                control.DAMAGE_DATA.TakeCollateralDamage(attacker, col, this);
            }

            CheckCollidingWeapons(col);
        }

        private CharacterControl CheckCollidingBodyparts(Collider collider)
        {
            if (control == null)
            {
                return null;
            }

            for (int i = 0; i < control.RAGDOLL_DATA.arrBodyParts.Length; i++)
            {
                if (control.RAGDOLL_DATA.arrBodyParts[i].Equals(collider))
                {
                    return null;
                }
            }

            var attacker = CharacterManager.Instance.GetCharacter(collider.transform.root.gameObject);

            if (attacker == null)
            {
                return null;
            }

            if (collider.gameObject == attacker.gameObject)
            {
                return null;
            }

            // add collider to dictionary
            control.DAMAGE_DATA.AddCollidersToDictionary(control.DAMAGE_DATA.collidingBodyParts_Dictionary, collider, this);

            return attacker;
        }

        private void CheckCollidingWeapons(Collider collider)
        {
            var w = collider.transform.root.gameObject.GetComponent<MeleeWeapon>();

            if (w == null)
            {
                return;
            }

            if (w.isThrown)
            {
                if (w.thrower != control)
                {
                    var info = new AttackCondition();
                    info.CopyInfo(control.DAMAGE_DATA.weaponThrow, control);

                    control.DAMAGE_DATA.damageTaken = new DamageTaken(
                        w.thrower,
                        control.DAMAGE_DATA.weaponThrow,
                        this,
                        null,
                        Vector3.zero);

                    control.DAMAGE_DATA.TakeDamage(info);

                    if (w.flyForward)
                    {
                        w.transform.rotation = Quaternion.Euler(0f, 90f, 45f);
                    }
                    else
                    {
                        w.transform.rotation = Quaternion.Euler(0f, -90f, 45f);
                    }

                    w.transform.parent = this.transform;

                    Vector3 offset = this.transform.position - w.weaponTip.transform.position;
                    w.transform.position += offset;

                    w.isThrown = false;
                    return;
                }
            }

            control.DAMAGE_DATA.AddCollidersToDictionary(control.DAMAGE_DATA.collidingWeapons_Dictionary, collider, this);
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