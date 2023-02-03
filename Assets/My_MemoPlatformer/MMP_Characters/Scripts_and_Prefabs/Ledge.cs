using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace My_MemoPlatformer
{
    public class Ledge : MonoBehaviour
    {
        public Vector3 offset;
        public Vector3 endPosition;

        public static bool IsLedge(GameObject obj)
        {
            if (obj.GetComponent<Ledge>() == null)
            {
                return false;
            }
            return true;
        }
    }
}