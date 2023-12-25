using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;


namespace My_MemoPlatformer
{

    public class TriggerDetector : MonoBehaviour
    {
        //public List<Collider> collidingParts = new List<Collider>();
        public CharacterControl control;

        public Vector3 lastPosition;
        public Quaternion lastRotation;

        private void Awake()
        {
            control = this.GetComponentInParent<CharacterControl>();
        }


        private void OnTriggerEnter(Collider col) //callback function whenever somth touches or enters or otuches raggdoll body parts
        {
            CheckCollidingBodyparts(col);
            CheckCollidingWeapons(col);
        }

        private void CheckCollidingBodyparts(Collider col)
        {
            if (control == null)
            {
                return;
            }

            if (control.RagdollData.bodyParts.Contains(col))  //touching own collider
            {
                return;
            }

            CharacterControl attacker = col.transform.root.GetComponent<CharacterControl>();

            if (attacker == null) //not a player, just a physical object
            {
                return;
            }
            //if we past two tests above, thats means its another character

            if (col.gameObject == attacker.gameObject) //not a boxcolllider itself in the top of hierarchy
            {
                return;
            }

            if (!control.animationProgress.collidingBodyParts.ContainsKey(this))
            {
                control.animationProgress.collidingBodyParts.Add(this, new List<Collider>());
            }

            if (!control.animationProgress.collidingBodyParts[this].Contains(col))
            {
                control.animationProgress.collidingBodyParts[this].Add(col);
            }
        }

        void CheckCollidingWeapons(Collider col)
        {
            var w = col.transform.root.gameObject.GetComponent<MeleeWeapon>();

            if (w == null)
            {
                return;
            }

            if (w.isThrown)
            {
                if (w.thrower != control) //not colliding himself
                {
                    var info = new AttackInfo();
                    info.CopyInfo(control.damageDetector.AxeThrow, control);

                    control.damageDetector_Data.damagedTrigger = this;
                    control.damageDetector_Data.attack = control.damageDetector.AxeThrow;
                    control.damageDetector_Data.attacker = w.thrower;
                    control.damageDetector_Data.attackingPart = w.thrower.rightHand_Attack;

                    control.damageDetector.TakeDamage(info);

                    if (w.flyForward)
                    {
                        w.transform.rotation = Quaternion.Euler(-90f, 0f, -180f);
                    }
                    else
                    {
                        w.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
                    }

                    w.transform.parent = this.transform;

                    var offset = this.transform.position - w.weaponTip.transform.position;
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
    }
}