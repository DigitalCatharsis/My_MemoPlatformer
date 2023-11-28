using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class LedgeChecker : MonoBehaviour
    {
        public bool isGrabbingLedge;
        public Vector3 ledgeCalibration = new Vector3();  //diffirence (offset) when we change character. (Bones changing)

        private CharacterControl _control;

        public LedgeCollider collider1; //bottom
        public LedgeCollider collider2; //top


        public List<string> ledgeTriggerStateNames = new List<string>();

        private void Start()
        {
            isGrabbingLedge = false;
            _control = GetComponentInParent<CharacterControl>();
        }
        private void FixedUpdate()
        {
            if (_control.skinnedMeshAnimator.GetBool(HashManager.Instance.dicMainParams[TransitionParameter.Grounded]))
            {
                if (_control.Rigid_Body.useGravity)
                {
                    isGrabbingLedge = false;
                }
            }

            if (IsLedgeGrabCondition())
            {
                ProcessLedgeGrab();
            }
        }

        private bool IsLedgeGrabCondition()
        {
            if (!_control.moveUp)
            {
                return false;
            }

            foreach(string s in ledgeTriggerStateNames)
            {
                if (_control.animationProgress.StateNameContains(s))
                {
                    return true;
                }
            }

            return false;
        }

        private void ProcessLedgeGrab()
        {
            if (!_control.skinnedMeshAnimator.GetBool(HashManager.Instance.dicMainParams[TransitionParameter.Grounded]))
            {
                foreach (var obj in collider1.collidedObjects)
                {
                    if (!collider2.collidedObjects.Contains(obj))
                    {
                        if (OffsetPosition(obj))
                        {
                            break;
                        }
                    }
                    else
                    {
                        isGrabbingLedge = false;
                    }
                }
            }
            else
            {
                isGrabbingLedge = false;
            }

            if (collider1.collidedObjects.Count == 0)
            {
                isGrabbingLedge = false;
            }
        }
        private bool OffsetPosition(GameObject platform)
        {
            var boxCollider = platform.GetComponent<BoxCollider>();

            if (boxCollider == null)
            {
                return false;
            }

            if (isGrabbingLedge)   //if already grabbing
            {
                return false;
            }

            isGrabbingLedge = true;
            _control.Rigid_Body.useGravity = false;
            _control.Rigid_Body.velocity = Vector3.zero;

            float y, z;

            y = platform.transform.position.y + (boxCollider.size.y * boxCollider.gameObject.transform.lossyScale.y / 2f);

            if (_control.IsFacingForward())
            {
                z = platform.transform.position.z - (boxCollider.size.z * boxCollider.gameObject.transform.lossyScale.z / 2f);
            }
            else
            {
                z = platform.transform.position.z + (boxCollider.size.z * boxCollider.gameObject.transform.lossyScale.z / 2f);
            }

            var plarformEdge = new Vector3(0f, y, z);

            var testingSphere = GameObject.Find("TestingSphere");
            testingSphere.transform.position = plarformEdge;

            if (_control.IsFacingForward())
            {
                _control.Rigid_Body.MovePosition(plarformEdge + ledgeCalibration);
            }
            else
            {
                _control.Rigid_Body.MovePosition(plarformEdge + new Vector3(0f, ledgeCalibration.y, -ledgeCalibration.z));
            }

            return true;
        }
    }
}