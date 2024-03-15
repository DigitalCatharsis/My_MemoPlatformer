using System.Collections.Generic;
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

        public override void OnComponentEnabled()
        {
            ledgeGrab_Data = new LedgeGrab_Data
            {
                upBlockingObjects = new List<Collider>(),
                isGrabbingLedge = false,
                DisableLedgeColliders = DisableLedgeColliders,
            };

            subComponentProcessor.ledgeGrab_Data = ledgeGrab_Data;
        }

        public override void OnUpdate()
        {

        }
        public override void OnFixedUpdate()
        {
            OnBeingGrounded();

            if (IsLedgeGrabCondition())
            {
                if (!control.skinnedMeshAnimator.GetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]))  //!grounded
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
                            ProcessPositionOffset(boxCollider);
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
            if (!control.skinnedMeshAnimator.GetBool(HashManager.Instance.arrMainParams[(int)MainParameterType.Grounded]))
            {
                if (control.rigidBody.useGravity)
                {
                    ledgeGrab_Data.isGrabbingLedge = false;
                }
            }
        }

        private bool IsLedgeGrabCondition()
        {
            if (!control.moveUp)
            {
                return false;
            }
            var info = control.skinnedMeshAnimator.GetCurrentAnimatorStateInfo(0);

            if (!HashManager.Instance.IsStateInCurrent_StateEnum<Ledge_Trigger_States>(control, info.shortNameHash))
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
                    //Debug.Log(collider.gameObject.name);
                    if (!Ledge.IsLedgeCollider(collider.gameObject)
                        && collider.transform.root.gameObject != control.gameObject
                        && control.RAGDOLL_DATA.GetBodypart(collider.gameObject.name)
                        )
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void ProcessPositionOffset(BoxCollider boxCollider)
        {
            ledgeGrab_Data.isGrabbingLedge = true;
            control.rigidBody.useGravity = false;
            control.rigidBody.velocity = Vector3.zero;

            int layer = 1 << 11; //ground

            RaycastHit hit;
            var startPoint = new Vector3(0f, colliderBot.GetComponent<BoxCollider>().bounds.min.y, colliderBot.transform.position.z);
            Physics.Raycast(startPoint, colliderBot.transform.forward, out hit, 3, layer);
            if (hit.point.z == 0)
            {
                Debug.DrawRay(colliderBot.transform.position, colliderBot.transform.forward, Color.cyan, 3f);
            }

            var platformEdge = new Vector3(0, boxCollider.bounds.max.y, hit.point.z);

            if (control.ROTATION_DATA.IsFacingForward())
            {
                control.rigidBody.MovePosition(platformEdge + ledgeCalibration);
            }
            else
            {
                control.rigidBody.MovePosition(platformEdge + new Vector3(0f, ledgeCalibration.y, -ledgeCalibration.z));
            }
        }

        public void DisableLedgeColliders()
        {
            colliderBot.GetComponent<BoxCollider>().enabled = false;
            colliderTop.GetComponent<BoxCollider>().enabled = false;
        }
    }
}