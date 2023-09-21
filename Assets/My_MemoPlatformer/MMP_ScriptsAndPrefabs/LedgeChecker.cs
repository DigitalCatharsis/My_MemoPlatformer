using UnityEngine;

namespace My_MemoPlatformer
{
    public class LedgeChecker : MonoBehaviour
    {
        public bool isGrabbongLedge;
        public Ledge grabbedLedge;
        public Vector3 ledgeCalibration = new Vector3();  //diffirence (offset) when we change character. (Bones changing)
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