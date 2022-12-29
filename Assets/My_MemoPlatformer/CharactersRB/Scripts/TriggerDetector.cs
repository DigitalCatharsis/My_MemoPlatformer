using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace My_MemoPlatformer
{

    public enum GeneralBodyPart
    {
        Upper,
        Lower,
        Arm,
        Leg,
    }


    public class TriggerDetector : MonoBehaviour
    {
        public GeneralBodyPart generalBodyPart;

        public List<Collider> collidingParts = new List<Collider>();
        private CharacterControl owner;

        private void Awake()
        {
            owner = this.GetComponentInParent<CharacterControl>();
        }


        private void OnTriggerEnter(Collider col) //callback function whenever somth touches or enters or otuches raggdoll body parts
        {


            if (owner.ragdollParts.Contains(col))
            {
                return;
            }

            CharacterControl attacker = col.transform.root.GetComponent<CharacterControl>();

            if (attacker == null)
            {
                return;
            }

            if (col.gameObject == attacker.gameObject) //not a boxcolllider itself
            {
                return;
            }

            if (!collidingParts.Contains(col))
            {
                collidingParts.Add(col);
            }

        }

        private void OnTriggerExit(Collider attacker)
        {
            if (collidingParts.Contains(attacker))
            {
                collidingParts.Remove(attacker);
            }
        }
    }
}