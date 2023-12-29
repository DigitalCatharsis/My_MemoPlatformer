using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class TriggerDetector : MonoBehaviour
    {
        [SerializeField] private bool _debug;
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
                TakeCollateralDamage(attacker, col);
            }

            CheckCollidingWeapons(col);
        }

        private CharacterControl CheckCollidingBodyparts(Collider col)
        {
            if (control == null)
            {
                return null;
            }

            for (int i = 0; i < control.RAGDOLL_DATA.arrBodyParts.Length; i++)
            {
                if (control.RAGDOLL_DATA.arrBodyParts[i].Equals(col))
                {
                    return null;
                }
            }

            var attacker = CharacterManager.Instance.GetCharacter(col.transform.root.gameObject);

            if (attacker == null)
            {
                return null;
            }

            if (col.gameObject == attacker.gameObject)
            {
                return null;
            }

            // add collider to dictionary

            if (!control.animationProgress.collidingBodyParts.ContainsKey(this))
            {
                control.animationProgress.collidingBodyParts.Add(this, new List<Collider>());
            }

            if (!control.animationProgress.collidingBodyParts[this].Contains(col))
            {
                control.animationProgress.collidingBodyParts[this].Add(col);
            }

            return attacker;
        }

        private void CheckCollidingWeapons(Collider col)
        {
            MeleeWeapon w = col.transform.root.gameObject.GetComponent<MeleeWeapon>();

            if (w == null)
            {
                return;
            }

            if (w.isThrown)
            {
                if (w.thrower != control)
                {
                    AttackCondition info = new AttackCondition();
                    info.CopyInfo(control.DAMAGE_DATA.AxeThrow, control);

                    control.DAMAGE_DATA.damageTaken = new DamageTaken(
                        w.thrower,
                        control.DAMAGE_DATA.AxeThrow,
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

            if (!control.animationProgress.collidingWeapons.ContainsKey(this))
            {
                control.animationProgress.collidingWeapons.Add(this, new List<Collider>());
            }

            if (!control.animationProgress.collidingWeapons[this].Contains(col))
            {
                control.animationProgress.collidingWeapons[this].Add(col);
            }
        }


        private void OnTriggerExit(Collider col)
        {
            CheckExitingBodypart(col);
            CheckExitingWeapons(col);
        }

        private void CheckExitingBodypart(Collider col)
        {
            if (control == null)
            {
                return;
            }

            if (control.animationProgress.collidingBodyParts.ContainsKey(this))
            {
                if (control.animationProgress.collidingBodyParts[this].Contains(col))
                {
                    control.animationProgress.collidingBodyParts[this].Remove(col);
                }

                if (control.animationProgress.collidingBodyParts[this].Count == 0)
                {
                    control.animationProgress.collidingBodyParts.Remove(this);
                }
            }
        }
        private void CheckExitingWeapons(Collider col)
        {
            if (control == null)
            {
                return;
            }

            if (control.animationProgress.collidingWeapons.ContainsKey(this))
            {
                if (control.animationProgress.collidingWeapons[this].Contains(col))
                {
                    control.animationProgress.collidingWeapons[this].Remove(col);
                }

                if (control.animationProgress.collidingWeapons[this].Count == 0)
                {
                    control.animationProgress.collidingWeapons.Remove(this);
                }
            }
        }

        private void TakeCollateralDamage(CharacterControl attacker, Collider col)
        {
            if (attacker.RAGDOLL_DATA.flyingRagdollData.isTriggered)
            {
                if (attacker.RAGDOLL_DATA.flyingRagdollData.attacker != control)
                {
                    var mag = Vector3.SqrMagnitude(col.attachedRigidbody.velocity);

                    if (_debug)
                    {
                        Debug.Log("incoming ragdoll: " + attacker.gameObject.name + "\n" + "Velocity: " + mag);
                    }

                    if (mag >= 10f)
                    {
                        control.DAMAGE_DATA.damageTaken = new DamageTaken(
                            attacker: null,
                            attack: null,
                            damage_TG: this,
                            damager: null,
                            incomingVelocity: col.attachedRigidbody.velocity);

                        control.DAMAGE_DATA.hp = 0;
                        control.RAGDOLL_DATA.ragdollTriggered = true;
                    }
                }
            }
        }
    }
}