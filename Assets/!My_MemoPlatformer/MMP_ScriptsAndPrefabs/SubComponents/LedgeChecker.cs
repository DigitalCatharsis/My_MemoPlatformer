using System;
using System.Collections.Generic;
using UnityEditor;
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
                DisableLedgeColliders = DisableLedgeColliders,
            };

            subComponentProcessor.ledgeGrab_Data = ledgeGrab_Data;
            subComponentProcessor.arrSubComponents[(int)SubComponentType.LEDGE_CHECKER] = this;
        }

        public override void OnUpdate()
        {

        }
        public override void OnFixedUpdate()
        {
            OnBeingGrounded();

            if (IsLedgeGrabCondition())
            {
                ProcessLedgeGrab();
            }
        }

        private void OnBeingGrounded()
        {
            if (Control.skinnedMeshAnimator.GetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]))
            {
                if (Control.RIGID_BODY.useGravity)
                {
                    ledgeGrab_Data.isGrabbingLedge = false;
                }
            }
        }

        private bool IsLedgeGrabCondition()
        {
            if (!Control.moveUp)
            {
                return false;
            }

            var info = Control.skinnedMeshAnimator.GetCurrentAnimatorStateInfo(0);

            if (!HashManager.Instance.IsStateInCurrent_StateEnum<Ledge_Trigger_States>(Control, info.shortNameHash))
            {
                return false;
            }

            if (CheckUpBlockingForLedgeGrabCondition())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool CheckUpBlockingForLedgeGrabCondition()
        {
            var start = Control.COLLISION_SPHERE_DATA.upSpheres[4];

            //draw DebugLine
            Debug.DrawRay(start.transform.position, Vector3.up, Color.red);

            //check collision
            RaycastHit hit;
            if (Physics.Raycast(start.transform.position, Vector3.up, out hit, 1))
            {
                if (!Ledge.IsLedgeCollider(hit.collider.gameObject))
                {
                    Debug.Log(hit.collider.transform.gameObject.ToString());
                    return true;
                }
                else
                {
                    return false;
                }
            }

            Debug.Log("nothing");
            return false;
        }

        private void ProcessLedgeGrab()
        {
            if (!Control.skinnedMeshAnimator.GetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]))
            {
                foreach (var collidedObject in collider1.collidedObjects)
                {
                    if (!collider2.collidedObjects.Contains(collidedObject))
                    {
                        if (OffsetPosition(collidedObject))
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
            //TODO: раздробить
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
            y = platform.transform.position.y + (boxCollider.transform.lossyScale.y / 2f);
            if (Control.ROTATION_DATA.IsFacingForward())
            {
                z = platform.transform.position.z - (boxCollider.gameObject.transform.lossyScale.z / 2f);
            }
            else
            {
                z = platform.transform.position.z + (boxCollider.gameObject.transform.lossyScale.z / 2f);
            }

            Vector3 platformEdge = new Vector3(0f, y, z);

            if (Control.ROTATION_DATA.IsFacingForward())
            {
                Control.RIGID_BODY.MovePosition(platformEdge + ledgeCalibration);
            }
            else
            {
                Control.RIGID_BODY.MovePosition(platformEdge + new Vector3(0f, ledgeCalibration.y, -ledgeCalibration.z));
            }

            return true;
        }

        public void DisableLedgeColliders()
        {
            collider1.GetComponent<BoxCollider>().enabled = false;
            collider2.GetComponent<BoxCollider>().enabled = false;
        }
    }
}