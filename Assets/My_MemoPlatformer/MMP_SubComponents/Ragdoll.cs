using UnityEngine;
using UnityEngine.AI;

namespace My_MemoPlatformer
{
    public class Ragdoll : SubComponent
    {
        private void Start()
        {
            control.procDict.Add(CharacterProc.RAGDOLL_ON, TurnOnRagdoll);
        }

        public override void OnFixedUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUpdate()
        {
            throw new System.NotImplementedException();
        }



        public void TurnOnRagdoll()
        {
            if (control.animationProgress.ragdollTriggered)
            {
                return;
            }

            control.animationProgress.ragdollTriggered = true;

            //change components layers from character to DeadBody to prevent unnessesary collisions.
            Transform[] arr = control.gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform t in arr)
            {
                t.gameObject.layer = LayerMask.NameToLayer(MMP_Layers.DeadBody.ToString());
            }

            //save bodypart positions to prevent teleporting
            foreach (Collider c in control.bodyParts)
            {
                TriggerDetector det = c.GetComponent<TriggerDetector>();
                det.lastPosition = c.gameObject.transform.localPosition;
                det.lastRotation = c.gameObject.transform.localRotation;
            }

            //turn off animator, avatar
            control.Rigid_Body.useGravity = false;
            control.Rigid_Body.velocity = Vector3.zero;
            control.gameObject.GetComponent<BoxCollider>().enabled = false;
            control.skinnedMeshAnimator.enabled = false;
            control.skinnedMeshAnimator.avatar = null;

            //Turn off ledge colliders
            control.procDict[CharacterProc.LEDGE_COLLIDERS_OFF]();

            //turn off ai
            if (control.aiController != null)
            {
                control.aiController.gameObject.SetActive(false);
                control.navMeshObstacle.carving = false; //we dont need carving when enemy is dead
            }

            //turn on ragdoll
            foreach (Collider c in control.bodyParts)
            {
                c.isTrigger = false;

                TriggerDetector det = c.GetComponent<TriggerDetector>();
                c.transform.localPosition = det.lastPosition;
                c.transform.localRotation = det.lastRotation;

                c.attachedRigidbody.velocity = Vector3.zero;
            }

            control.AddForceToDamagedPart(false);
        }
    }
}
