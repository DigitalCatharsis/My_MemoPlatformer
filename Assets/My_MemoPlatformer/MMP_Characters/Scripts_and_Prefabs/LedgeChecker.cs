using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class LedgeChecker : MonoBehaviour
    {
        public bool isGrabbongLedge;
        public Ledge grabbedLedge;
        private Ledge _checkLedge = null;

        private void OnTriggerEnter(Collider other)
        {
            _checkLedge = other.gameObject.GetComponent<Ledge>();
            if (_checkLedge != null )
            {
                isGrabbongLedge = true;
                grabbedLedge = _checkLedge;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            _checkLedge = other.gameObject.GetComponent<Ledge>();
            if (_checkLedge != null)
            {
                isGrabbongLedge = false;
                //grabbedLedge = null;
            }
        }
    }
}