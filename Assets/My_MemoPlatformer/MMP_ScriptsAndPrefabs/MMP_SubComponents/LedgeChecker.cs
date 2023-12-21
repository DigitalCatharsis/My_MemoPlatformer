using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

namespace My_MemoPlatformer
{
    public class LedgeChecker : SubComponent
    {
        public LedgeGrab_Data ledgeGrab_Data;

        [SerializeField] private Vector3 ledgeCalibration = new Vector3();  //diffirence (offset) when we change character. (Bones changing)
        [SerializeField] private LedgeCollider collider1; //bottom
        [SerializeField] private LedgeCollider collider2; //top
        [SerializeField] private List<string> ledgeTriggerStateNames = new List<string>();

        private void Start()
        {
            ledgeGrab_Data = new LedgeGrab_Data
            {
                isGrabbingLedge = false,
                LedgeCollidersOff = LedgeCollidersOff,
            };

            subComponentProcessor.ledgeGrabData = ledgeGrab_Data;
            subComponentProcessor.componentsDictionary.Add(SubComponents.LEDGECHECKER, this);
        }

        public override void OnUpdate()
        {
            //Барбара меня накажет, простите :c
        }
        public override void OnFixedUpdate()
        {
            if (control.skinnedMeshAnimator.GetBool(HashManager.Instance.dicMainParams[TransitionParameter.Grounded]))
            {
                if (control.Rigid_Body.useGravity)
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
            if (!control.moveUp)
            {
                return false;
            }

            foreach(string s in ledgeTriggerStateNames)
            {
                if (control.animationProgress.StateNameContains(s))
                {
                    return true;
                }
            }

            return false;
        }

        private void ProcessLedgeGrab()
        {
            if (!control.skinnedMeshAnimator.GetBool(HashManager.Instance.dicMainParams[TransitionParameter.Grounded]))
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
            control.Rigid_Body.useGravity = false;
            control.Rigid_Body.velocity = Vector3.zero;

            float y, z;

            y = platform.transform.position.y + (boxCollider.size.y * boxCollider.gameObject.transform.lossyScale.y / 2f);

            if (control.IsFacingForward())
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

            if (control.IsFacingForward())
            {
                control.Rigid_Body.MovePosition(plarformEdge + ledgeCalibration);
            }
            else
            {
                control.Rigid_Body.MovePosition(plarformEdge + new Vector3(0f, ledgeCalibration.y, -ledgeCalibration.z));
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