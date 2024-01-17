using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class LedgeChecker : SubComponent
    {
        public LedgeGrab_Data ledgeGrab_Data;

        [Header("Ledge Setup")]
        [SerializeField] private Vector3 ledgeCalibration = new Vector3();  //diffirence (offset) when we change character. (Bones changing)
        public LedgeCollider collider1; //bottom
        public LedgeCollider collider2; //top

        private void Start()
        {
            ledgeGrab_Data = new LedgeGrab_Data
            {
                isGrabbingLedge = false,
                LedgeCollidersOff = LedgeCollidersOff,
            };

            subComponentProcessor.ledgeGrab_Data = ledgeGrab_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.LEDGE_CHECKER] = this;
        }

        public override void OnUpdate()
        {

        }
        public override void OnFixedUpdate()
        {
            if (Control.skinnedMeshAnimator.GetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]))
            {
                if (Control.RIGID_BODY.useGravity)
                {
                    ledgeGrab_Data.isGrabbingLedge = false;
                }
            }

            if (IsLedgeGrabCondition())
            {
                ProcessLedgeGrab();
            }
        }

        private bool IsLedgeGrabCondition()
        {
            if (!Control.moveUp)
            {
                return false;
            }

            for (int i = 0; i < HashManager.Instance.arrLedgeTriggerStates.Length; i++)
            {
                var info = Control.skinnedMeshAnimator.GetCurrentAnimatorStateInfo(0);
                if (info.shortNameHash == HashManager.Instance.arrLedgeTriggerStates[i])
                {
                    return true;
                }
            }

            return false;
        }

        private void ProcessLedgeGrab()
        {
            if (!Control.skinnedMeshAnimator.GetBool(
                HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]))
            {
                foreach (GameObject obj in collider1.collidedObjects)
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
                        ledgeGrab_Data.isGrabbingLedge = false;
                    }
                }
            }
            else
            {
                ledgeGrab_Data.isGrabbingLedge = false;
            }

            if (collider1.collidedObjects.Count == 0)
            {
                ledgeGrab_Data.isGrabbingLedge = false;
            }
        }
        private bool OffsetPosition(GameObject platform)
        {
            var boxCollider = platform.GetComponent<BoxCollider>();

            if (boxCollider == null)
            {
                return false;
            }

            if (ledgeGrab_Data.isGrabbingLedge)   //if already grabbing
            {
                return false;
            }

            ledgeGrab_Data.isGrabbingLedge = true;
            Control.RIGID_BODY.useGravity = false;
            Control.RIGID_BODY.velocity = Vector3.zero;

            float y, z;
            y = platform.transform.position.y + (boxCollider.size.y / 2f);
            if (Control.ROTATION_DATA.IsFacingForward())
            {
                z = platform.transform.position.z - (boxCollider.size.z * boxCollider.gameObject.transform.lossyScale.z / 2f);
            }
            else
            {
                z = platform.transform.position.z + (boxCollider.size.z * boxCollider.gameObject.transform.lossyScale.z / 2f);
            }

            Vector3 platformEdge = new Vector3(0f, y, z);

            if (Control.ROTATION_DATA.IsFacingForward())
            {
                Control.RIGID_BODY.MovePosition(
                    platformEdge + ledgeCalibration);
            }
            else
            {
                Control.RIGID_BODY.MovePosition(
                    platformEdge + new Vector3(0f, ledgeCalibration.y, -ledgeCalibration.z));
            }

            return true;
        }

        public void LedgeCollidersOff()
        {
            collider1.GetComponent<BoxCollider>().enabled = false;
            collider2.GetComponent<BoxCollider>().enabled = false;
        }
    }
}