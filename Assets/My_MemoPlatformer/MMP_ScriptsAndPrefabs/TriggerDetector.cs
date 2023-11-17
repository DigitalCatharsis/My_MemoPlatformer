using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{

    public class TriggerDetector : MonoBehaviour
    {
        //public List<Collider> collidingParts = new List<Collider>();
        private CharacterControl _control;

        public Vector3 lastPosition;
        public Quaternion lastRotation;

        private void Awake()
        {
            _control = this.GetComponentInParent<CharacterControl>();
        }


        private void OnTriggerEnter(Collider col) //callback function whenever somth touches or enters or otuches raggdoll body parts
        {
            CheckCollidingBodyparts(col);
            CheckCollidingWeapons(col);
        }

        private void CheckCollidingBodyparts(Collider col)
        {
            if (_control.ragdollParts.Contains(col))  //touching own collider
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

            if (!_control.animationProgress.collidingBodyParts.ContainsKey(this))
            {
                _control.animationProgress.collidingBodyParts.Add(this, new List<Collider>());
            }

            if (!_control.animationProgress.collidingBodyParts[this].Contains(col))
            {
                _control.animationProgress.collidingBodyParts[this].Add(col);
            }
        }

        void CheckCollidingWeapons(Collider col)
        {
            if (col.transform.root.gameObject.GetComponent<MeleeWeapon>() == null)
            {
                return;
            }

            if (!_control.animationProgress.collidingWeapons.ContainsKey(this))
            {
                _control.animationProgress.collidingWeapons.Add(this, new List<Collider>());
            }

            if (!_control.animationProgress.collidingWeapons[this].Contains(col))
            {
                _control.animationProgress.collidingWeapons[this].Add(col);
            }
        }

        private void OnTriggerExit(Collider col)
        {
            CheckExitingBodypart(col);
            CheckExitingWeapons(col);
        }

        private void CheckExitingBodypart(Collider col)
        {
            if (_control.animationProgress.collidingBodyParts.ContainsKey(this))
            {
                if (_control.animationProgress.collidingBodyParts[this].Contains(col))
                {
                    _control.animationProgress.collidingBodyParts[this].Remove(col);
                }

                if (_control.animationProgress.collidingBodyParts[this].Count == 0)
                {
                    _control.animationProgress.collidingBodyParts.Remove(this);
                }
            }
        }
        private void CheckExitingWeapons(Collider col)
        {
            if (_control.animationProgress.collidingWeapons.ContainsKey(this))
            {
                if (_control.animationProgress.collidingWeapons[this].Contains(col))
                {
                    _control.animationProgress.collidingWeapons[this].Remove(col);
                }

                if (_control.animationProgress.collidingWeapons[this].Count == 0)
                {
                    _control.animationProgress.collidingWeapons.Remove(this);
                }
            }
        }
    }
}