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
        private CharacterControl _owner;

        public Vector3 lastPosition;
        public Quaternion lastRotation;

        private void Awake()
        {
            _owner = this.GetComponentInParent<CharacterControl>();
        }


        private void OnTriggerEnter(Collider col) //callback function whenever somth touches or enters or otuches raggdoll body parts
        {
            if (_owner.ragdollParts.Contains(col))  //touching own collider
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