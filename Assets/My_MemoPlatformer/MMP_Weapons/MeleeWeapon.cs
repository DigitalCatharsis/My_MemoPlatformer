using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class MeleeWeapon : MonoBehaviour
    {
        public CharacterControl control;
        public Vector3 customPosition = new Vector3();
        public Vector3 customRotation = new Vector3();
        public BoxCollider pickUpCollider;
        public BoxCollider attackCollider;
        public TriggerDetector triggerDetector;

        [Header("WeaponThrow")]
        public Vector3 throwOffset;
        public bool isThrown;
        public bool flyForward;
        public float flightSpeed;
        public float rotationSpeed;
        public CharacterControl thrower;
        public GameObject weaponTip;

        private void Start()
        {
            isThrown = false;
        }

        private void Update()
        {
            if (control != null)
            {
                pickUpCollider.enabled = false;
                attackCollider.enabled = true;
            }
            else
            {
                pickUpCollider.enabled = true;
                attackCollider.enabled = false;
            }
        }

        private void FixedUpdate()
        {
            if (isThrown)
            {
                if (flyForward)
                {
                    this.transform.position += (Vector3.forward * flightSpeed * Time.deltaTime);
                }
                else
                {
                    this.transform.position -= (Vector3.forward * flightSpeed * Time.deltaTime);
                }

                this.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
            }
        }

        public static bool IsWeapon(GameObject obj)
        {
            if (obj.transform.root.gameObject.GetComponent<MeleeWeapon>() != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DropWeapon()
        {
            var weapon = control.animationProgress.HoldingWeapon;

            if (weapon != null)
            {
                weapon.transform.parent = null;

                if (control.IsFacingForward())
                {
                    weapon.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
                }
                else
                {
                    weapon.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                }

                RemoveWeaponFromDictionary(control);

                weapon.transform.position = control.transform.position + Vector3.up * 0.015f;

                control.animationProgress.HoldingWeapon = null;
                control = null;

                weapon.triggerDetector.control = null;
            }
        }

        public void ThrowWeapon()
        {
            var weapon = control.animationProgress.HoldingWeapon;

            if (weapon != null)
            {
                weapon.transform.parent = null;

                if (control.IsFacingForward())
                {
                    weapon.transform.rotation = Quaternion.Euler(90f, 0, 0f);
                }
                else
                {
                    weapon.transform.rotation = Quaternion.Euler(-90f, 0, 0f);
                }

                flyForward = control.IsFacingForward();

                weapon.transform.position = control.transform.position + Vector3.up * throwOffset.y;
                weapon.transform.position += control.transform.forward * throwOffset.z;

                thrower = control;
                control.animationProgress.HoldingWeapon = null;
                control = null;
                weapon.triggerDetector.control = null;

                isThrown = true;

                RemoveWeaponFromDictionary(thrower);
            }
        }

        public void RemoveWeaponFromDictionary(CharacterControl c)
        {
            foreach (var col in c.RagdollData.bodyParts)
            {
                var t = col.GetComponent<TriggerDetector>();

                if (t != null)
                {
                    ProcRemove(c.animationProgress.collidingWeapons, t);
                    ProcRemove(c.animationProgress.collidingBodyParts, t);
                }
            }
        }

        private void ProcRemove(Dictionary<TriggerDetector, List<Collider>> d, TriggerDetector t)
        {
            if (d.ContainsKey(t))
            {
                if (d[t].Contains(pickUpCollider))
                {
                    d[t].Remove(pickUpCollider);
                }

                if (d[t].Contains(attackCollider))
                {
                    d[t].Remove(attackCollider);
                }

                if (d[t].Count == 0)
                {
                    d.Remove(t);
                }
            }
        }
    }
}
