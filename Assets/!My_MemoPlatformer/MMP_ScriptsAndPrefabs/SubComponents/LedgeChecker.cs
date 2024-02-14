using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class LedgeChecker : SubComponent
    {
        public LedgeGrab_Data ledgeGrab_Data;

        [Header("Ledge Setup")]
        [SerializeField] private Vector3 ledgeCalibration = new Vector3();  //diffirence (offset) when we change character. (Bones changing)
        public LedgeCollider colliderTop; //top
        public LedgeCollider colliderBot; //bottom
        public GameObject upBlockChecker; //bottom

        private void Start()
        {
            ledgeGrab_Data = new LedgeGrab_Data
            {
                upBlockingObjects = new List<Collider>(),
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
                if (!Control.skinnedMeshAnimator.GetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]))  //!grounded
                {
                    foreach (var collidedObject in colliderBot.collidedObjects)
                    {
                        var boxCollider = collidedObject.GetComponent<BoxCollider>();

                        if (boxCollider == null)
                        {
                            break;
                        }

                        if (ledgeGrab_Data.isGrabbingLedge)   //if already grabbing
                        {
                            break;
                        }

                        if (!colliderTop.collidedObjects.Contains(collidedObject))
                        {
                            ProcessPositionOffset(collidedObject, boxCollider);
                            break;

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

                if (colliderBot.collidedObjects.Count == 0)
                {
                    ledgeGrab_Data.isGrabbingLedge = false;
                }
            }
        }

        private void OnBeingGrounded()
        {
            if (!Control.skinnedMeshAnimator.GetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]))
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

            if (IsUpBlocking())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void OnDrawGizmos()
        {

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(upBlockChecker.transform.position, upBlockChecker.GetComponent<SphereCollider>().radius);
        }

        private bool IsUpBlocking()
        {
            ledgeGrab_Data.upBlockingObjects.Clear();
            ledgeGrab_Data.upBlockingObjects.AddRange(Physics.OverlapSphere(
                    upBlockChecker.transform.position,
                    upBlockChecker.GetComponent<SphereCollider>().radius,
                    layerMask: LayerMask.GetMask("GROUND")));

            if (ledgeGrab_Data.upBlockingObjects.Count > 0)
            {
                foreach (var collider in ledgeGrab_Data.upBlockingObjects)
                {
                    Debug.Log(collider.gameObject.name);
                    if (!Ledge.IsLedgeCollider(collider.gameObject)
                        && collider.transform.root.gameObject != Control.gameObject
                        && Control.RAGDOLL_DATA.GetBodypart(collider.gameObject.name)
                        )
                    {
                        Debug.Log("true");
                        return true;
                    }
                }
            }

            Debug.Log("false2");
            return false;

            //draw DebugLine
            //Debug.DrawRay(start.transform.position, Vector3.up, Color.red);

            ////check collision
            //RaycastHit hit;
            //if (Physics.Raycast(start.transform.position, Vector3.up, out hit, 1, layerMask: 13))   //13 visual no Raycast
            //{
            //    if (!Ledge.IsLedgeCollider(hit.collider.gameObject))
            //    {
            //        if (this.transform.root.Find(hit.collider.gameObject.name) != null)
            //        {
            //            return true;
            //        }

            //        if (DebugContainer_Data.Instance.debug_Ledges)
            //        {
            //            Debug.Log(hit.collider.transform.gameObject.ToString());
            //        }

            //        return false;
            //        //Debug.Break();
            //    }
            //    else
            //    {
            //        Debug.Log(hit.collider.gameObject.ToString());
            //        return false;
            //    }
            //}

            ////Debug.Log("nothing");
            //return false;
        }
        private void ProcessPositionOffset(GameObject platform, BoxCollider boxCollider)
        {
            //TODO: ����������

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

            var platformEdge = new Vector3(0f, y, z);

            if (Control.ROTATION_DATA.IsFacingForward())
            {
                Control.RIGID_BODY.MovePosition(platformEdge + ledgeCalibration);
            }
            else
            {
                Control.RIGID_BODY.MovePosition(platformEdge + new Vector3(0f, ledgeCalibration.y, -ledgeCalibration.z));
            }
        }

        public void DisableLedgeColliders()
        {
            colliderBot.GetComponent<BoxCollider>().enabled = false;
            colliderTop.GetComponent<BoxCollider>().enabled = false;
        }
    }
}