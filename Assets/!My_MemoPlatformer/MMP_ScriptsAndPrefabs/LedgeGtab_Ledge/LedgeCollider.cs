using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class LedgeCollider : MonoBehaviour
    {
        public List<GameObject> collidedObjects = new List<GameObject>();

        private void OnTriggerEnter(Collider other)
        {
            if (!Ledge.IsCharacter(other.gameObject) && !MeleeWeapon.IsWeapon(other.gameObject))
            {
                if (!collidedObjects.Contains(other.gameObject))
                {
                    collidedObjects.Add(other.gameObject);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!Ledge.IsCharacter(other.gameObject) && !MeleeWeapon.IsWeapon(other.gameObject))
            {
                if (collidedObjects.Contains(other.gameObject))
                {
                    collidedObjects.Remove(other.gameObject);
                }
            }
        }
    }
}
